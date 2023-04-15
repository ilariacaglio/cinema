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
    [Area("Admin")]
    public class PrenotazioneController : Controller
    {
        private readonly AppDbContext _context;

        public PrenotazioneController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Prenotazione
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Prenotazioni.Include(p => p.IdUtenteNavigation).Include(p => p.Spettacolo);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Prenotazione/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Prenotazioni == null)
            {
                return NotFound();
            }

            var prenotazione = await _context.Prenotazioni
                .Include(p => p.IdUtenteNavigation)
                .Include(p => p.Spettacolo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (prenotazione == null)
            {
                return NotFound();
            }

            return View(prenotazione);
        }

        // GET: Prenotazione/Create
        public IActionResult Create()
        {
            ViewData["IdUtente"] = new SelectList(_context.Utenti, "Mail", "Mail");
            ViewData["DataS"] = new SelectList(_context.Spettacoli, "Data", "Data");
            return View();
        }

        // POST: Prenotazione/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DataS,OraS,IdSala,IdUtente")] Prenotazione prenotazione)
        {
            if (ModelState.IsValid)
            {
                _context.Add(prenotazione);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdUtente"] = new SelectList(_context.Utenti, "Mail", "Mail", prenotazione.IdUtente);
            ViewData["DataS"] = new SelectList(_context.Spettacoli, "Data", "Data", prenotazione.DataS);
            return View(prenotazione);
        }

        // GET: Prenotazione/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Prenotazioni == null)
            {
                return NotFound();
            }

            var prenotazione = await _context.Prenotazioni.FindAsync(id);
            if (prenotazione == null)
            {
                return NotFound();
            }
            ViewData["IdUtente"] = new SelectList(_context.Utenti, "Mail", "Mail", prenotazione.IdUtente);
            ViewData["DataS"] = new SelectList(_context.Spettacoli, "Data", "Data", prenotazione.DataS);
            return View(prenotazione);
        }

        // POST: Prenotazione/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DataS,OraS,IdSala,IdUtente")] Prenotazione prenotazione)
        {
            if (id != prenotazione.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(prenotazione);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PrenotazioneExists(prenotazione.Id))
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
            ViewData["IdUtente"] = new SelectList(_context.Utenti, "Mail", "Mail", prenotazione.IdUtente);
            ViewData["DataS"] = new SelectList(_context.Spettacoli, "Data", "Data", prenotazione.DataS);
            return View(prenotazione);
        }

        // GET: Prenotazione/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Prenotazioni == null)
            {
                return NotFound();
            }

            var prenotazione = await _context.Prenotazioni
                .Include(p => p.IdUtenteNavigation)
                .Include(p => p.Spettacolo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (prenotazione == null)
            {
                return NotFound();
            }

            return View(prenotazione);
        }

        // POST: Prenotazione/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Prenotazioni == null)
            {
                return Problem("Entity set 'AppDbContext.Prenotazioni'  is null.");
            }
            var prenotazione = await _context.Prenotazioni.FindAsync(id);
            if (prenotazione != null)
            {
                _context.Prenotazioni.Remove(prenotazione);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PrenotazioneExists(int id)
        {
          return (_context.Prenotazioni?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
