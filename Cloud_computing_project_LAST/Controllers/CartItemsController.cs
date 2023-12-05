using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Cloud_computing_project_LAST.Data;
using Cloud_computing_project_LAST.Models;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using Cloud_computing_project_LAST.Data.Migrations;

namespace Cloud_computing_project_LAST.Controllers
{
    public class CartItemsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IHttpContextAccessor _httpContextAccessor;

        public CartItemsController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(int productId,string productName, int quantity)
        {
            // check if the amount is valid
            var products =  _context.Product.ToList();
            var menues = _context.Menu.ToList();
            var product = menues.Find(x => x.Name == productName);
            var productShop = products.Find(x => x.Name == productName);
            if (productShop != null)
            {
                if (quantity > productShop.InStock)
                {
                    return Json(new { success = false, message = $"only {productShop.InStock} left in the stock" });
                }
                product = new Menu
                {
                    Id = productShop.Id,
                    Name = productShop.Name,
                    Description = productShop.Description,
                    ImageUrl = productShop.ImageUrl,
                    Price = productShop.Price,
                };
                
            }
           
            if(User.Identity.IsAuthenticated)
            {
                try
                {
                    var cartItem = await _context.CartItem.FindAsync(productId);

                    if (cartItem == null)
                    {
                        return Json(new { success = false, message = "Item not found" });
                    }

                    // Get the current subtotal before updating the quantity
                    var currentSubtotal = cartItem.Price;

                    // Update the quantity
                    cartItem.Amount = quantity;
                    cartItem.Price = quantity * product.Price;
                    _context.Update(cartItem);
                    await _context.SaveChangesAsync();

                    // Calculate the new subtotal
                   

                    var carts = _context.Cart.ToList();
                    var cart = carts.Find(C => C.Id == cartItem.cartId);
                    cart.TotalPrice = cart.TotalPrice + cartItem.Price - currentSubtotal;
                    var newSubtotal = cart.TotalPrice;
                    _context.Cart.Update(cart);
                    await _context.SaveChangesAsync();

                    return Json(new { success = true, message = "Quantity updated successfully", newSubTotal = newSubtotal , newPrice = cartItem.Price});
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "Error updating quantity", error = ex.Message });
                }
            }
            else
            {
                try
                {
                    var httpContext = _httpContextAccessor.HttpContext;
                    var guestCart = httpContext.Session.GetString("GuestCart");

                    var newSubtotal = 0.0;
                    var price = 0.0;
                    CartItem cartItem = null;
                    if (!string.IsNullOrEmpty(guestCart))
                    {
                        var tempCart = JsonConvert.DeserializeObject<Models.Cart>(guestCart);
                        cartItem = tempCart.CartItem.Find(c => c.Id == productId);
                        // Get the current subtotal before updating the quantity
                        var currentSubtotal = cartItem.Price;

                        // Update the quantity
                        cartItem.Amount = quantity;
                        cartItem.Price = quantity * product.Price;
                        price = cartItem.Price; 
                        // Calculate the new subtotal
                        

                        tempCart.TotalPrice = tempCart.TotalPrice - currentSubtotal + cartItem.Price; 
                        newSubtotal = tempCart.TotalPrice;
                        httpContext.Session.SetString("GuestCart", JsonConvert.SerializeObject(tempCart));
                    }

                    if (cartItem == null)
                    {
                        return Json(new { success = false, message = "Item not found" });
                    }

                    
                    return Json(new { success = true, message = "Quantity updated successfully", newSubTotal = newSubtotal , newPrice = price});
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "Error updating quantity", error = ex.Message });
                }
            }
               
           
        }



        [HttpPost]
        public async Task<IActionResult> RemoveItem(int productId)
        {
            if (User.Identity.IsAuthenticated)
            {
                try
                {
                    var cartItem = await _context.CartItem.FindAsync(productId);

                    if (cartItem == null)
                    {
                        return Json(new { success = false, message = "Item not found" });
                    }

                    _context.CartItem.Remove(cartItem);
                    _context.SaveChanges();

                    var carts =_context.Cart.ToList();
                    var cart = carts.Find(c => c.Id == cartItem.cartId);
                    cart.Quantity -= 1;
                    cart.TotalPrice -= cartItem.Price;

                    _context.Cart.Update(cart);
                    _context.SaveChanges();

                    return Json(new { success = true, message = "Item removed successfully", newTotal = cart.TotalPrice ,quantity = cart.Quantity});
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "Error removing item", error = ex.Message });
                }
            }
            else
            {
                try
                {
                    var httpContext = _httpContextAccessor.HttpContext;
                    var guestCart = httpContext.Session.GetString("GuestCart");

                    var price = 0.0;
                    CartItem cartItem = null;
                    var amount = 0;
                    if (!string.IsNullOrEmpty(guestCart))
                    {
                        var tempCart = JsonConvert.DeserializeObject<Models.Cart>(guestCart);
                        cartItem = tempCart.CartItem.Find(c => c.Id == productId);
                        tempCart.CartItem.Remove(cartItem);
                        tempCart.Quantity -= 1;
                        tempCart.TotalPrice -= cartItem.Price;
                        httpContext.Session.SetString("GuestCart", JsonConvert.SerializeObject(tempCart));
                        price = tempCart.TotalPrice;
                        amount = tempCart.Quantity;
                    }

                    if (cartItem == null)
                    {
                        return Json(new { success = false, message = "Item not found" });
                    }

                    

                    return Json(new { success = true, message = "Item removed successfully", newTotal = price , quantity = amount});
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "Error removing item", error = ex.Message });
                }
            }
        }

        public async Task<IActionResult> Cart()
        {
            var cartItems = new List<CartItem>();

            if (User.Identity.IsAuthenticated)
            {
                var carts = await _context.Cart.Include(c => c.CartItem).ToListAsync();
                var cart = carts.Find(e => e.userId == User.Identity.Name);
                if (cart != null)
                {
                    cartItems = cart.CartItem.ToList();
                }
            }
            else
            {
                var httpContext = _httpContextAccessor.HttpContext;
                var guestCart = httpContext.Session.GetString("GuestCart");

                if (!string.IsNullOrEmpty(guestCart))
                {
                    var tempCart = JsonConvert.DeserializeObject<Models.Cart>(guestCart);
                    cartItems = tempCart.CartItem;
                }
            }

            // Calculate subtotal and total
            var subtotal = cartItems.Sum(item => item.Price);
            var total = subtotal; // You can add additional logic for discounts, taxes, etc.

            // Pass the subtotal and total to the view using ViewData
            ViewData["Subtotal"] = subtotal;
            ViewData["Total"] = total;

            return View(cartItems);
        }

      



        // GET: CartItems
        public async Task<IActionResult> Index()
        {
              return _context.CartItem != null ? 
                          View(await _context.CartItem.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.CartItem'  is null.");

        }

        // GET: CartItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CartItem == null)
            {
                return NotFound();
            }

            var cartItem = await _context.CartItem
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cartItem == null)
            {
                return NotFound();
            }

            return View(cartItem);
        }

        // GET: CartItems/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CartItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,InStock,ImageUrl,Price,Amount")] CartItem cartItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cartItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cartItem);
        }

        // GET: CartItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CartItem == null)
            {
                return NotFound();
            }

            var cartItem = await _context.CartItem.FindAsync(id);
            if (cartItem == null)
            {
                return NotFound();
            }
            return View(cartItem);
        }

        // POST: CartItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,InStock,ImageUrl,Price,Amount")] CartItem cartItem)
        {
            if (id != cartItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cartItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CartItemExists(cartItem.Id))
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
            return View(cartItem);
        }

        // GET: CartItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CartItem == null)
            {
                return NotFound();
            }

            var cartItem = await _context.CartItem
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cartItem == null)
            {
                return NotFound();
            }

            return View(cartItem);
        }

        // POST: CartItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CartItem == null)
            {
                return Problem("Entity set 'ApplicationDbContext.CartItem'  is null.");
            }
            var cartItem = await _context.CartItem.FindAsync(id);
            if (cartItem != null)
            {
                _context.CartItem.Remove(cartItem);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CartItemExists(int id)
        {
          return (_context.CartItem?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        

    }
}
