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
    public class SpettacoloController : Controller
    {
        private readonly AppDbContext _context;

        public SpettacoloController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Spettacolo
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Spettacoli.Include(s => s.IdFilmNavigation).Include(s => s.IdSalaNavigation);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Spettacolo/Details/5
        public async Task<IActionResult> Details(DateOnly? id)
        {
            if (id == null || _context.Spettacoli == null)
            {
                return NotFound();
            }

            var spettacolo = await _context.Spettacoli
                .Include(s => s.IdFilmNavigation)
                .Include(s => s.IdSalaNavigation)
                .FirstOrDefaultAsync(m => m.Data == id);
            if (spettacolo == null)
            {
                return NotFound();
            }

            return View(spettacolo);
        }

        // GET: Spettacolo/Create
        public IActionResult Create()
        {
            ViewData["IdFilm"] = new SelectList(_context.Film, "Id", "Id");
            ViewData["IdSala"] = new SelectList(_context.Sale, "Id", "Id");
            return View();
        }

        // POST: Spettacolo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Data,Ora,IdFilm,IdSala")] Spettacolo spettacolo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(spettacolo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdFilm"] = new SelectList(_context.Film, "Id", "Id", spettacolo.IdFilm);
            ViewData["IdSala"] = new SelectList(_context.Sale, "Id", "Id", spettacolo.IdSala);
            return View(spettacolo);
        }

        // GET: Spettacolo/Edit/5
        public async Task<IActionResult> Edit(DateOnly? id)
        {
            if (id == null || _context.Spettacoli == null)
            {
                return NotFound();
            }

            var spettacolo = await _context.Spettacoli.FindAsync(id);
            if (spettacolo == null)
            {
                return NotFound();
            }
            ViewData["IdFilm"] = new SelectList(_context.Film, "Id", "Id", spettacolo.IdFilm);
            ViewData["IdSala"] = new SelectList(_context.Sale, "Id", "Id", spettacolo.IdSala);
            return View(spettacolo);
        }

        // POST: Spettacolo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DateOnly id, [Bind("Data,Ora,IdFilm,IdSala")] Spettacolo spettacolo)
        {
            if (id != spettacolo.Data)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(spettacolo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SpettacoloExists(spettacolo.Data))
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
            ViewData["IdFilm"] = new SelectList(_context.Film, "Id", "Id", spettacolo.IdFilm);
            ViewData["IdSala"] = new SelectList(_context.Sale, "Id", "Id", spettacolo.IdSala);
            return View(spettacolo);
        }

        // GET: Spettacolo/Delete/5
        public async Task<IActionResult> Delete(DateOnly? id)
        {
            if (id == null || _context.Spettacoli == null)
            {
                return NotFound();
            }

            var spettacolo = await _context.Spettacoli
                .Include(s => s.IdFilmNavigation)
                .Include(s => s.IdSalaNavigation)
                .FirstOrDefaultAsync(m => m.Data == id);
            if (spettacolo == null)
            {
                return NotFound();
            }

            return View(spettacolo);
        }

        // POST: Spettacolo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(DateOnly id)
        {
            if (_context.Spettacoli == null)
            {
                return Problem("Entity set 'AppDbContext.Spettacoli'  is null.");
            }
            var spettacolo = await _context.Spettacoli.FindAsync(id);
            if (spettacolo != null)
            {
                _context.Spettacoli.Remove(spettacolo);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SpettacoloExists(DateOnly id)
        {
          return (_context.Spettacoli?.Any(e => e.Data == id)).GetValueOrDefault();
        }
    }
}
