using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Cloud_computing_project_LAST.Data;
using Cloud_computing_project_LAST.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
namespace Cloud_computing_project_LAST.Controllers
{
    public class CartsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
       
        
        public CartsController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: Carts
        public async Task<IActionResult> Index()
        {
            return _context.Cart != null ?
                        View(await _context.Cart.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Cart'  is null.");
        }

        // GET: Carts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Cart == null)
            {
                return NotFound();
            }

            var cart = await _context.Cart
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // GET: Carts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Carts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,userId,Quantity,TotalPrice")] Cart cart)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cart);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(cart);
        }

        // GET: Carts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Cart == null)
            {
                return NotFound();
            }

            var cart = await _context.Cart.FindAsync(id);
            if (cart == null)
            {
                return NotFound();
            }
            return View(cart);
        }

        // POST: Carts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,userId,Quantity,TotalPrice")] Cart cart)
        {
            if (id != cart.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cart);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CartExists(cart.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(cart);
        }

        // GET: Carts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Cart == null)
            {
                return NotFound();
            }

            var cart = await _context.Cart
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // POST: Carts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Cart == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Cart'  is null.");
            }
            var cart = await _context.Cart.FindAsync(id);
            if (cart != null)
            {
                _context.Cart.Remove(cart);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CartExists(int id)
        {
            return (_context.Cart?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        [HttpPost]
        public async Task<ActionResult> AddToCart(int productId, int quantity)
        {
            
                if (quantity == 0)
                    return Json(new { success = true, message = "Can not add 0 quantity" });
                var product = await _context.Product.FindAsync(productId);
                if(quantity > product.InStock)
                    return Json(new { success = false, message = $"only {product.InStock} left in the stock" });
                if (_context.Cart == null)
                {
                    return Problem("Entity set 'ApplicationDbContext.Cart'  is null.");
                }
            var item = new CartItem
            {
                Name = product.Name,
                Description = product.Description,
                ImageUrl = product.ImageUrl,
                Amount = quantity,
                Price = product.Price * quantity,
                InStock = 0,
            };
            if (User.Identity.IsAuthenticated)
            {
                var carts = await _context.Cart.ToListAsync();

                var cart = carts.Find(e => e.userId == User.Identity.Name);

                
                var cartItems = await _context.CartItem.ToListAsync();

                var oldItem = cartItems.Find(e => e.Name == product.Name);

                if (oldItem == null)
                {
                    cart.CartItem.Add(item);
                    _context.SaveChanges();
                    cart.TotalPrice += item.Price;
                    cart.Quantity += 1;
                    _context.Update(cart);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    _context.CartItem.Remove(oldItem);
                    _context.SaveChanges();
                    cart.CartItem.Add(item);
                    _context.SaveChanges();
                    cart.TotalPrice = cart.TotalPrice - oldItem.Price + item.Price;
                    _context.Update(cart);
                    await _context.SaveChangesAsync();
                }
            }
            else
            {
                var httpContext = _httpContextAccessor.HttpContext;

                var guestCart = httpContext.Session.GetString("GuestCart");

                if (string.IsNullOrEmpty(guestCart))
                {
                    // Create a new temporary cart for guest users
                    var cart = new Cart(); // Assuming Cart is your cart model
                    cart.userId = Guid.NewGuid().ToString(); // Temporary identifier for guest users
                    cart.CartItem = new List<CartItem>(); // Initialize cart items list
                    cart.CartItem.Add(item);
                    cart.Quantity = 1;
                    cart.TotalPrice = item.Price;
                    httpContext.Session.SetString("GuestCart", JsonConvert.SerializeObject(cart)); // Store the cart in session for guest users
                }
                else
                {
                    // Retrieve existing cart from session and deserialize it
                    var tempCart = JsonConvert.DeserializeObject<Cart>(guestCart);
                    var oldItem = tempCart.CartItem.Find(e => e.Name == product.Name);
                    
                    if(oldItem == null)
                    {
                        tempCart.CartItem.Add(item);
                        tempCart.Quantity += 1;
                        tempCart.TotalPrice += item.Price;
                    }
                    else
                    {
                        tempCart.CartItem.Remove(oldItem);
                        tempCart.CartItem.Add(item);
                        tempCart.TotalPrice = tempCart.TotalPrice - oldItem.Price + item.Price;
                    }

                    httpContext.Session.SetString("GuestCart", JsonConvert.SerializeObject(tempCart));
                }

            }
            return Json(new { success = true, message = "Item added to cart successfully." });
        }



    }
}

