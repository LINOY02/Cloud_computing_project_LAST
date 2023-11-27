using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Cloud_computing_project_LAST.Data;
using Cloud_computing_project_LAST.Models;

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
    }
}
