using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Cinema.DataAccess;
using Cinema.Models;

namespace Cinema.Controllers
{
    public class GenereController : Controller
    {
        private readonly AppDbContext _context;

        public GenereController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Genere
        public async Task<IActionResult> Index()
        {
              return _context.Generi != null ? 
                          View(await _context.Generi.ToListAsync()) :
                          Problem("Entity set 'AppDbContext.Generi'  is null.");
        }

        // GET: Genere/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Generi == null)
            {
                return NotFound();
            }

            var genere = await _context.Generi
                .FirstOrDefaultAsync(m => m.Id == id);
            if (genere == null)
            {
                return NotFound();
            }

            return View(genere);
        }

        // GET: Genere/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Genere/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome")] Genere genere)
        {
            if (ModelState.IsValid)
            {
                _context.Add(genere);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(genere);
        }

        // GET: Genere/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Generi == null)
            {
                return NotFound();
            }

            var genere = await _context.Generi.FindAsync(id);
            if (genere == null)
            {
                return NotFound();
            }
            return View(genere);
        }

        // POST: Genere/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome")] Genere genere)
        {
            if (id != genere.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(genere);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GenereExists(genere.Id))
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
            return View(genere);
        }

        // GET: Genere/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Generi == null)
            {
                return NotFound();
            }

            var genere = await _context.Generi
                .FirstOrDefaultAsync(m => m.Id == id);
            if (genere == null)
            {
                return NotFound();
            }

            return View(genere);
        }

        // POST: Genere/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Generi == null)
            {
                return Problem("Entity set 'AppDbContext.Generi'  is null.");
            }
            var genere = await _context.Generi.FindAsync(id);
            if (genere != null)
            {
                _context.Generi.Remove(genere);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GenereExists(int id)
        {
          return (_context.Generi?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
