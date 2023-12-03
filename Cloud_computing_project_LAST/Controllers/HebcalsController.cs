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
    public class HebcalsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HebcalsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Hebcals
        public async Task<IActionResult> Index()
        {
              return _context.Hebcal != null ? 
                          View(await _context.Hebcal.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Hebcal'  is null.");
        }

        // GET: Hebcals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Hebcal == null)
            {
                return NotFound();
            }

            var hebcal = await _context.Hebcal
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hebcal == null)
            {
                return NotFound();
            }

            return View(hebcal);
        }

        // GET: Hebcals/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Hebcals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,orderId,hy,hm,hd,hebrew,events")] Hebcal hebcal)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hebcal);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(hebcal);
        }

        // GET: Hebcals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Hebcal == null)
            {
                return NotFound();
            }

            var hebcal = await _context.Hebcal.FindAsync(id);
            if (hebcal == null)
            {
                return NotFound();
            }
            return View(hebcal);
        }

        // POST: Hebcals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,orderId,hy,hm,hd,hebrew,events")] Hebcal hebcal)
        {
            if (id != hebcal.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hebcal);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HebcalExists(hebcal.Id))
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
            return View(hebcal);
        }

        // GET: Hebcals/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Hebcal == null)
            {
                return NotFound();
            }

            var hebcal = await _context.Hebcal
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hebcal == null)
            {
                return NotFound();
            }

            return View(hebcal);
        }

        // POST: Hebcals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Hebcal == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Hebcal'  is null.");
            }
            var hebcal = await _context.Hebcal.FindAsync(id);
            if (hebcal != null)
            {
                _context.Hebcal.Remove(hebcal);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HebcalExists(int id)
        {
          return (_context.Hebcal?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
