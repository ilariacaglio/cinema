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
    public class ComprendeController : Controller
    {
        private readonly AppDbContext _context;

        public ComprendeController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Comprende
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Comprende.Include(c => c.Id).Include(c => c.Prenotazione);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Comprende/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Comprende == null)
            {
                return NotFound();
            }

            var comprende = await _context.Comprende
                .Include(c => c.Id)
                .Include(c => c.Prenotazione)
                .FirstOrDefaultAsync(m => m.IdPosto == id);
            if (comprende == null)
            {
                return NotFound();
            }

            return View(comprende);
        }

        // GET: Comprende/Create
        public IActionResult Create()
        {
            ViewData["IdPosto"] = new SelectList(_context.Posti, "Id", "Id");
            ViewData["IdPrenotazione"] = new SelectList(_context.Prenotazioni, "Id", "IdUtente");
            return View();
        }

        // POST: Comprende/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPosto,IdSala,IdPrenotazione,DataS,OraS,IdUtente")] Comprende comprende)
        {
            if (ModelState.IsValid)
            {
                _context.Add(comprende);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdPosto"] = new SelectList(_context.Posti, "Id", "Id", comprende.IdPosto);
            ViewData["IdPrenotazione"] = new SelectList(_context.Prenotazioni, "Id", "IdUtente", comprende.IdPrenotazione);
            return View(comprende);
        }

        // GET: Comprende/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Comprende == null)
            {
                return NotFound();
            }

            var comprende = await _context.Comprende.FindAsync(id);
            if (comprende == null)
            {
                return NotFound();
            }
            ViewData["IdPosto"] = new SelectList(_context.Posti, "Id", "Id", comprende.IdPosto);
            ViewData["IdPrenotazione"] = new SelectList(_context.Prenotazioni, "Id", "IdUtente", comprende.IdPrenotazione);
            return View(comprende);
        }

        // POST: Comprende/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdPosto,IdSala,IdPrenotazione,DataS,OraS,IdUtente")] Comprende comprende)
        {
            if (id != comprende.IdPosto)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(comprende);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ComprendeExists(comprende.IdPosto))
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
            ViewData["IdPosto"] = new SelectList(_context.Posti, "Id", "Id", comprende.IdPosto);
            ViewData["IdPrenotazione"] = new SelectList(_context.Prenotazioni, "Id", "IdUtente", comprende.IdPrenotazione);
            return View(comprende);
        }

        // GET: Comprende/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Comprende == null)
            {
                return NotFound();
            }

            var comprende = await _context.Comprende
                .Include(c => c.Id)
                .Include(c => c.Prenotazione)
                .FirstOrDefaultAsync(m => m.IdPosto == id);
            if (comprende == null)
            {
                return NotFound();
            }

            return View(comprende);
        }

        // POST: Comprende/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Comprende == null)
            {
                return Problem("Entity set 'AppDbContext.Comprende'  is null.");
            }
            var comprende = await _context.Comprende.FindAsync(id);
            if (comprende != null)
            {
                _context.Comprende.Remove(comprende);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ComprendeExists(int id)
        {
          return (_context.Comprende?.Any(e => e.IdPosto == id)).GetValueOrDefault();
        }
    }
}
