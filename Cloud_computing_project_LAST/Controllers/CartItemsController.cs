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
        public async Task<IActionResult> UpdateQuantity(int productId, int quantity)
        {
            // check if the amount is valid
            var product = await _context.Product.FindAsync(productId);
           
                try
                {
                    var cartItem = await _context.CartItem.FindAsync(productId);

                    if (cartItem == null)
                    {
                        return Json(new { success = false, message = "Item not found" });
                    }

                    // Get the current subtotal before updating the quantity
                    var currentSubtotal = cartItem.Price * cartItem.Amount;

                    // Update the quantity
                    cartItem.Amount = quantity;
                    _context.Update(cartItem);
                    await _context.SaveChangesAsync();

                    // Calculate the new subtotal
                    var newSubtotal = cartItem.Price * cartItem.Amount;

                    return Json(new { success = true, message = "Quantity updated successfully", newSubtotal = newSubtotal });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "Error updating quantity", error = ex.Message });
                }
           
        }



        [HttpPost]
        public async Task<IActionResult> RemoveItem(int productId)
        {
            try
            {
                var cartItem = await _context.CartItem.FindAsync(productId);

                if (cartItem == null)
                {
                    return Json(new { success = false, message = "Item not found" });
                }

                _context.CartItem.Remove(cartItem);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Item removed successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error removing item", error = ex.Message });
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
                    var tempCart = JsonConvert.DeserializeObject<Cart>(guestCart);
                    cartItems = tempCart.CartItem;
                }
            }

            // Calculate subtotal and total
            var subtotal = cartItems.Sum(item => item.Price * item.Amount);
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
