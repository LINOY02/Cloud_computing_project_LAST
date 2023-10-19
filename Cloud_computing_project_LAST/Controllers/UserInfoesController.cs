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
    public class UserInfoesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserInfoesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: UserInfoes
        public async Task<IActionResult> Index()
        {
              return _context.UsersInfo != null ? 
                          View(await _context.UsersInfo.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.UsersInfo'  is null.");
        }

        // GET: UserInfoes/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.UsersInfo == null)
            {
                return NotFound();
            }

            var userInfo = await _context.UsersInfo
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userInfo == null)
            {
                return NotFound();
            }

            return View(userInfo);
        }

        // GET: UserInfoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UserInfoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,PhoneNumber,City,Street,StreetNum,ZIPCode")] UserInfo userInfo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userInfo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(userInfo);
        }

        // GET: UserInfoes/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.UsersInfo == null)
            {
                return NotFound();
            }

            var userInfo = await _context.UsersInfo.FindAsync(id);
            if (userInfo == null)
            {
                return NotFound();
            }
            return View(userInfo);
        }

        // POST: UserInfoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,FirstName,LastName,PhoneNumber,City,Street,StreetNum,ZIPCode")] UserInfo userInfo)
        {
            if (id != userInfo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userInfo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserInfoExists(userInfo.Id))
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
            return View(userInfo);
        }

        // GET: UserInfoes/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.UsersInfo == null)
            {
                return NotFound();
            }

            var userInfo = await _context.UsersInfo
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userInfo == null)
            {
                return NotFound();
            }

            return View(userInfo);
        }

        // POST: UserInfoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.UsersInfo == null)
            {
                return Problem("Entity set 'ApplicationDbContext.UsersInfo'  is null.");
            }
            var userInfo = await _context.UsersInfo.FindAsync(id);
            if (userInfo != null)
            {
                _context.UsersInfo.Remove(userInfo);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserInfoExists(string id)
        {
          return (_context.UsersInfo?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
