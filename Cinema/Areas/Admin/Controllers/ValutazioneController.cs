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
    public class ValutazioneController : Controller
    {
        private readonly AppDbContext _context;

        public ValutazioneController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Valutazione
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Valutazioni.Include(v => v.IdFilmNavigation).Include(v => v.IdUtenteNavigation);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Valutazione/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Valutazioni == null)
            {
                return NotFound();
            }

            var valutazione = await _context.Valutazioni
                .Include(v => v.IdFilmNavigation)
                .Include(v => v.IdUtenteNavigation)
                .FirstOrDefaultAsync(m => m.IdUtente == id);
            if (valutazione == null)
            {
                return NotFound();
            }

            return View(valutazione);
        }

        // GET: Valutazione/Create
        public IActionResult Create()
        {
            ViewData["IdFilm"] = new SelectList(_context.Film, "Id", "Id");
            ViewData["IdUtente"] = new SelectList(_context.Utenti, "Mail", "Mail");
            return View();
        }

        // POST: Valutazione/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdUtente,IdFilm,Voto")] Valutazione valutazione)
        {
            if (ModelState.IsValid)
            {
                _context.Add(valutazione);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdFilm"] = new SelectList(_context.Film, "Id", "Id", valutazione.IdFilm);
            ViewData["IdUtente"] = new SelectList(_context.Utenti, "Mail", "Mail", valutazione.IdUtente);
            return View(valutazione);
        }

        // GET: Valutazione/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Valutazioni == null)
            {
                return NotFound();
            }

            var valutazione = await _context.Valutazioni.FindAsync(id);
            if (valutazione == null)
            {
                return NotFound();
            }
            ViewData["IdFilm"] = new SelectList(_context.Film, "Id", "Id", valutazione.IdFilm);
            ViewData["IdUtente"] = new SelectList(_context.Utenti, "Mail", "Mail", valutazione.IdUtente);
            return View(valutazione);
        }

        // POST: Valutazione/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("IdUtente,IdFilm,Voto")] Valutazione valutazione)
        {
            if (id != valutazione.IdUtente)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(valutazione);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ValutazioneExists(valutazione.IdUtente))
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
            ViewData["IdFilm"] = new SelectList(_context.Film, "Id", "Id", valutazione.IdFilm);
            ViewData["IdUtente"] = new SelectList(_context.Utenti, "Mail", "Mail", valutazione.IdUtente);
            return View(valutazione);
        }

        // GET: Valutazione/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Valutazioni == null)
            {
                return NotFound();
            }

            var valutazione = await _context.Valutazioni
                .Include(v => v.IdFilmNavigation)
                .Include(v => v.IdUtenteNavigation)
                .FirstOrDefaultAsync(m => m.IdUtente == id);
            if (valutazione == null)
            {
                return NotFound();
            }

            return View(valutazione);
        }

        // POST: Valutazione/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Valutazioni == null)
            {
                return Problem("Entity set 'AppDbContext.Valutazioni'  is null.");
            }
            var valutazione = await _context.Valutazioni.FindAsync(id);
            if (valutazione != null)
            {
                _context.Valutazioni.Remove(valutazione);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ValutazioneExists(string id)
        {
          return (_context.Valutazioni?.Any(e => e.IdUtente == id)).GetValueOrDefault();
        }
    }
}
