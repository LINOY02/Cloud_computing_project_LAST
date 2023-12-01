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

namespace Cloud_computing_project_LAST.Controllers
{
    public class OrderrsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderrsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Orderrs
        public async Task<IActionResult> Index()
        {
              return _context.Orderr != null ? 
                          View(await _context.Orderr.ToListAsync()) :
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
        public IActionResult Create()
        {
            return View();
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
                _context.Add(orderr);
                await _context.SaveChangesAsync();
                await SendOrderConfirmationEmail(orderr);
                return RedirectToAction(nameof(Index));
            }
            return View(orderr);
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
            var orders = _context.Order?.Where(orderr => orderr.OrderDate >= start && orderr.OrderDate <= end).ToList();

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

