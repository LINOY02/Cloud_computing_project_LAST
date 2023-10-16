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
    public class CafesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CafesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Cafes
        public async Task<IActionResult> Index()
        { 
            return _context.Cafe != null ? 
                          View(await _context.Cafe.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Cafe'  is null.");
        }

        // GET: Cafes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Cafe == null)
            {
                return NotFound();
            }

            var cafe = await _context.Cafe
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cafe == null)
            {
                return NotFound();
            }

            return View(cafe);
        }

        // GET: Cafes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Cafes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,ImageUrl")] Cafe cafe)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cafe);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cafe);
        }

        // GET: Cafes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Cafe == null)
            {
                return NotFound();
            }

            var cafe = await _context.Cafe.FindAsync(id);
            if (cafe == null)
            {
                return NotFound();
            }
            return View(cafe);
        }

        // POST: Cafes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,ImageUrl")] Cafe cafe)
        {
            if (id != cafe.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cafe);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CafeExists(cafe.Id))
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
            return View(cafe);
        }

        // GET: Cafes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Cafe == null)
            {
                return NotFound();
            }

            var cafe = await _context.Cafe
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cafe == null)
            {
                return NotFound();
            }

            return View(cafe);
        }

        // POST: Cafes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Cafe == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Cafe'  is null.");
            }
            var cafe = await _context.Cafe.FindAsync(id);
            if (cafe != null)
            {
                _context.Cafe.Remove(cafe);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CafeExists(int id)
        {
          return (_context.Cafe?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
