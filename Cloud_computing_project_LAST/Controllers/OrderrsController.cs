using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Cloud_computing_project_LAST.Data;
using Cloud_computing_project_LAST.Models;
using System.Net.Mail;
using System.Net;
using Cloud_computing_project_LAST.Data.Migrations;

using Newtonsoft.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis;
using System.Data.Entity;

namespace Cloud_computing_project_LAST.Controllers
{
    public class OrderrsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HebcalService _hebcalService = new HebcalService();
        private readonly WeatherService _weatherService = new WeatherService();


        public OrderrsController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: Orderrs
        public async Task<IActionResult> Index()
        {
              return _context.Orderr != null ? 
                          View( _context.Orderr.ToList()) :
                          Problem("Entity set 'ApplicationDbContext.Orderr'  is null.");
        }

        // GET: Orderrs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Orderr == null)
            {
                return NotFound();
            }

            var orderr = await _context.Orderr
                .FirstOrDefaultAsync(m => m.Id == id);
            if (orderr == null)
            {
                return NotFound();
            }

            return View(orderr);
        }

        // GET: Orderrs/Create
        public async Task<IActionResult> Create()
        {
            Orderr orderr = null;
            if (ModelState.IsValid)
            {
                if (User.Identity.IsAuthenticated)
                {
                    var user = await _context.UsersInfo.FindAsync(User.Identity.Name);
                    var carts = _context.Cart.ToList();
                    var cart = carts.Find(e => e.userId == User.Identity.Name);
                    orderr = CreateOrderFromUser(user);
                }
                if (User.Identity.IsAuthenticated)
                {
                    var carts = _context.Cart.ToList();
                    var cart = carts.Find(e => e.userId == User.Identity.Name);
                    orderr.TotalPrice = cart.TotalPrice;

                }
                else
                {
                    var httpContext = _httpContextAccessor.HttpContext;
                    var guestCart = httpContext.Session.GetString("GuestCart");
                    var cart = JsonConvert.DeserializeObject<Models.Cart>(guestCart);
                    orderr.TotalPrice = cart.TotalPrice;
                }
            }
            return View(orderr);
        }

        // POST: Orderrs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,OrderDate,DeliveryDate,TotalPrice,Name,LastName,City,Address,streetNum,ZIPCode,PhoneNumber,Email,Info")] Orderr orderr)
        {
            if (ModelState.IsValid)
            {
                orderr.OrderDate = DateTime.Now;
                orderr.DeliveryDate = GetNextHourFromCurrentTime();
               
                _context.Add(orderr);
                await _context.SaveChangesAsync();
                await SendOrderConfirmationEmail(orderr);
                ViewData["OrderDate"] = orderr.OrderDate;
                ViewData["DeliveryDate"] = orderr.DeliveryDate;

                var orders = _context.Orderr.ToList();
                orderr = orders.Find(e => e.Email == User.Identity.Name);
                var hebcal = await _hebcalService.HebcalRoot();
                var hebdate = JsonConvert.DeserializeObject<Hebcal>(hebcal);
                hebdate.orderId = orderr.Id;
                _context.Hebcal.Add(hebdate);
                await _context.SaveChangesAsync();

                var weather = await _weatherService.GetWeatherForCity(orderr.City);
                var weatherCity = JsonConvert.DeserializeObject<Weather>(weather);
                weatherCity.orderId = orderr.Id;
                _context.Weather.Add(weatherCity);
                await _context.SaveChangesAsync();

                if (User.Identity.IsAuthenticated)
                {
                    var carts = _context.Cart.ToList();
                    var cart = carts.Find(e => e.userId == User.Identity.Name);
                    var items = _context.CartItem.ToList();
                    var cartItems = items.FindAll(e => e.cartId == cart.Id);
                    foreach (CartItem cartItem in cartItems)
                    {
                        var orderItem = new OrderItem
                        { 
                            orderId = orderr.Id,    
                            Name = cartItem.Name,
                            Description = cartItem.Description,
                            ImageUrl = cartItem.ImageUrl,
                            Price = cartItem.Price,
                            Amount = cartItem.Amount,
                        };
                        _context.OrderItem.Add(orderItem);
                        await _context.SaveChangesAsync();
                        _context.CartItem.Remove(cartItem);
                        await _context.SaveChangesAsync();
                    }
                    cart.Quantity = 0;
                    cart.TotalPrice = 0;
                    _context.Cart.Update(cart);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    var httpContext = _httpContextAccessor.HttpContext;
                    var guestCart = httpContext.Session.GetString("GuestCart");
                    httpContext.Session.SetString("GuestCart", null);
                }

                return RedirectToAction(nameof(Index));
            }

            return View(orderr);
        }

        private Orderr CreateOrderFromUser(UserInfo user)
        {
            return new Orderr
            {
                Name = user.FirstName,
                LastName = user.LastName,
                City = user.City,
                Address = user.Street,
                streetNum = int.Parse(user.StreetNum),
                ZIPCode = user.ZIPCode,
                PhoneNumber = user.PhoneNumber,
                Email = User.Identity.Name,
                //TotalPrice = price
            };
        }

        private DateTime GetNextHourFromCurrentTime()
        {
            var currentDateTime = DateTime.Now;
            var nextHourDateTime = currentDateTime.AddHours(1);
            nextHourDateTime = new DateTime(nextHourDateTime.Year, nextHourDateTime.Month, 
                nextHourDateTime.Day,nextHourDateTime.Hour, 0, 0);
            return nextHourDateTime;
        }

        // GET: Orderrs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Orderr == null)
            {
                return NotFound();
            }

            var orderr = await _context.Orderr.FindAsync(id);
            if (orderr == null)
            {
                return NotFound();
            }
            return View(orderr);
        }

        // POST: Orderrs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,OrderDate,DeliveryDate,TotalPrice,Name,LastName,City,Address,streetNum,ZIPCode,PhoneNumber,Email,Info")] Orderr orderr)
        {
            if (id != orderr.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orderr);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderrExists(orderr.Id))
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
            return View(orderr);
        }

        // GET: Orderrs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Orderr == null)
            {
                return NotFound();
            }

            var orderr = await _context.Orderr
                .FirstOrDefaultAsync(m => m.Id == id);
            if (orderr == null)
            {
                return NotFound();
            }

            return View(orderr);
        }

        // POST: Orderrs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Orderr == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Orderr'  is null.");
            }
            var orderr = await _context.Orderr.FindAsync(id);
            if (orderr != null)
            {
                _context.Orderr.Remove(orderr);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderrExists(int id)
        {
          return (_context.Orderr?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private async Task SendOrderConfirmationEmail(Orderr orderr)
        {
            // Replace these values with your SMTP server details
            string smtpServer = "smtp.gmail.com";
            int smtpPort = 587;
            string smtpUsername = "caffena100@gmail.com";
            string smtpPassword = "zybc owcy vprg vmcb";

            using (var client = new SmtpClient(smtpServer, smtpPort))
            {
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                client.EnableSsl = true;

                var message = new MailMessage
                {
                    From = new MailAddress("caffena100@gmail.com"),
                    Subject = "Order Confirmation",
                    Body = $" Dear {orderr.Name}, Your order successfully received. Your total price is {orderr.TotalPrice:C}.",
                    IsBodyHtml = false
                };
                message.To.Add(orderr.Email);
                await client.SendMailAsync(message);
            }
        }
        public IActionResult GraphCreate()
        {
            return View();
        }
        public IActionResult Graph(DateTime? start, DateTime? end)
        {
            var orderCounts = new List<int>
    ();
            var orders = _context.Orderr?.Where(orderr => orderr.OrderDate >= start && orderr.OrderDate <= end).ToList();

            // Prepare data for the view model
            var dateLabels = orders.Select(orderr => orderr.OrderDate?.ToShortDateString()).Distinct().ToList();
            var totalPrices = new List<double>
                ();

            foreach (var dateLabel in dateLabels)
            {
                orderCounts.Add(orders.Count(order => order.OrderDate?.ToShortDateString() == dateLabel));
                totalPrices.Add(orders.Where(order => order.OrderDate?.ToShortDateString() == dateLabel).Sum(orderr => orderr.TotalPrice));
            }

            var viewModel = new OrderGraphViewModel
            {
                DateLabels = dateLabels,
                TotalPrices = totalPrices,
                OrderCounts = orderCounts
            };

            return View(viewModel); // Pass the view model to the view
        }
    }
}

