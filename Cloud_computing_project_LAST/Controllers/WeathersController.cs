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
    public class WeathersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WeathersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Weathers
        public async Task<IActionResult> Index()
        {
              return _context.Weather != null ? 
                          View(await _context.Weather.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Weather'  is null.");
        }

        // GET: Weathers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Weather == null)
            {
                return NotFound();
            }

            var weather = await _context.Weather
                .FirstOrDefaultAsync(m => m.Id == id);
            if (weather == null)
            {
                return NotFound();
            }

            return View(weather);
        }

        // GET: Weathers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Weathers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,orderId,Temp,FeelsLike,TempMin,TempMax,Pressure,Humidity,Main,Description")] Weather weather)
        {
            if (ModelState.IsValid)
            {
                _context.Add(weather);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(weather);
        }

        // GET: Weathers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Weather == null)
            {
                return NotFound();
            }

            var weather = await _context.Weather.FindAsync(id);
            if (weather == null)
            {
                return NotFound();
            }
            return View(weather);
        }

        // POST: Weathers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,orderId,Temp,FeelsLike,TempMin,TempMax,Pressure,Humidity,Main,Description")] Weather weather)
        {
            if (id != weather.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(weather);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WeatherExists(weather.Id))
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
            return View(weather);
        }

        // GET: Weathers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Weather == null)
            {
                return NotFound();
            }

            var weather = await _context.Weather
                .FirstOrDefaultAsync(m => m.Id == id);
            if (weather == null)
            {
                return NotFound();
            }

            return View(weather);
        }

        // POST: Weathers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Weather == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Weather'  is null.");
            }
            var weather = await _context.Weather.FindAsync(id);
            if (weather != null)
            {
                _context.Weather.Remove(weather);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WeatherExists(int id)
        {
          return (_context.Weather?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
