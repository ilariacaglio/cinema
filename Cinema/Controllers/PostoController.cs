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
    public class PostoController : Controller
    {
        private readonly AppDbContext _context;

        public PostoController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Posto
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Posti.Include(p => p.IdSalaNavigation);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Posto/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Posti == null)
            {
                return NotFound();
            }

            var posto = await _context.Posti
                .Include(p => p.IdSalaNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (posto == null)
            {
                return NotFound();
            }

            return View(posto);
        }

        // GET: Posto/Create
        public IActionResult Create()
        {
            ViewData["IdSala"] = new SelectList(_context.Sale, "Id", "Id");
            return View();
        }

        // POST: Posto/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Fila,Numero,Costo,IdSala")] Posto posto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(posto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdSala"] = new SelectList(_context.Sale, "Id", "Id", posto.IdSala);
            return View(posto);
        }

        // GET: Posto/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Posti == null)
            {
                return NotFound();
            }

            var posto = await _context.Posti.FindAsync(id);
            if (posto == null)
            {
                return NotFound();
            }
            ViewData["IdSala"] = new SelectList(_context.Sale, "Id", "Id", posto.IdSala);
            return View(posto);
        }

        // POST: Posto/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Fila,Numero,Costo,IdSala")] Posto posto)
        {
            if (id != posto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(posto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostoExists(posto.Id))
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
            ViewData["IdSala"] = new SelectList(_context.Sale, "Id", "Id", posto.IdSala);
            return View(posto);
        }

        // GET: Posto/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Posti == null)
            {
                return NotFound();
            }

            var posto = await _context.Posti
                .Include(p => p.IdSalaNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (posto == null)
            {
                return NotFound();
            }

            return View(posto);
        }

        // POST: Posto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Posti == null)
            {
                return Problem("Entity set 'AppDbContext.Posti'  is null.");
            }
            var posto = await _context.Posti.FindAsync(id);
            if (posto != null)
            {
                _context.Posti.Remove(posto);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostoExists(int id)
        {
          return (_context.Posti?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
