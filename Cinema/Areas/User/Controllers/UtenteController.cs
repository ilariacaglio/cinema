using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Cinema.DataAccess;
using Cinema.Models;
using Cinema.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Identity;

namespace Cinema.Controllers
{
    [Area("User")]
    public class UtenteController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<Utente> _userManager;

        public UtenteController(IUnitOfWork unitOfWork, UserManager<Utente> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        //restituisce tutti i film già visti dall'utente
        public async Task<IActionResult> FilmUtente() {
            //id utente
            string userId = await GetCurrentUserId();

            //recupera le prenotazioni
            var prenotazioni = _unitOfWork.Prenotazione.GetAll().Where(p => p.IdUtente == userId).ToList();

            //recupera i film dagli spettacoli
            HashSet<Film> films = new HashSet<Film>();
            foreach (var item in prenotazioni)
            {
                var spettacolo = _unitOfWork.Spettacolo.GetFirstOrDefault(item.DataS, item.OraS, item.IdSala);
                if (spettacolo != null)
                {
                    if (DateTime.Now >= new DateTime(spettacolo.Data.Year, spettacolo.Data.Month,spettacolo.Data.Day,spettacolo.Ora.AddHours(1).Hour, spettacolo.Ora.Minute, 0))
                    {
                        var f = _unitOfWork.Film.GetFirstOrDefault(spettacolo.IdFilm);
                        if (f != null)
                            films.Add(f);
                    }
                }
            }
            return View(films);
        }

        [HttpGet]
        public async Task<string> GetCurrentUserId()
        {
            Utente usr = await GetCurrentUserAsync();
            return usr?.Id;
        }
        private Task<Utente> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);
        //        // GET: Utente
        //        public async Task<IActionResult> Index()
        //        {
        //              return _context.Utenti != null ? 
        //                          View(await _context.Utenti.ToListAsync()) :
        //                          Problem("Entity set 'AppDbContext.Utenti'  is null.");
        //        }

        //        // GET: Utente/Details/5
        //        public async Task<IActionResult> Details(string id)
        //        {
        //            if (id == null || _context.Utenti == null)
        //            {
        //                return NotFound();
        //            }

        //            var utente = await _context.Utenti
        //                .FirstOrDefaultAsync(m => m.Email == id);
        //            if (utente == null)
        //            {
        //                return NotFound();
        //            }

        //            return View(utente);
        //        }

        //        // GET: Utente/Create
        //        public IActionResult Create()
        //        {
        //            return View();
        //        }

        //        // POST: Utente/Create
        //        // To protect from overposting attacks, enable the specific properties you want to bind to.
        //        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //        [HttpPost]
        //        [ValidateAntiForgeryToken]
        //        public async Task<IActionResult> Create([Bind("Cognome,Nome,Mail,Password,Sesso,Nascita,Residenza")] Utente utente)
        //        {
        //            if (ModelState.IsValid)
        //            {
        //                _context.Add(utente);
        //                await _context.SaveChangesAsync();
        //                return RedirectToAction(nameof(Index));
        //            }
        //            return View(utente);
        //        }

        //        // GET: Utente/Edit/5
        //        public async Task<IActionResult> Edit(string id)
        //        {
        //            if (id == null || _context.Utenti == null)
        //            {
        //                return NotFound();
        //            }

        //            var utente = await _context.Utenti.FindAsync(id);
        //            if (utente == null)
        //            {
        //                return NotFound();
        //            }
        //            return View(utente);
        //        }

        //        // POST: Utente/Edit/5
        //        // To protect from overposting attacks, enable the specific properties you want to bind to.
        //        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //        [HttpPost]
        //        [ValidateAntiForgeryToken]
        //        public async Task<IActionResult> Edit(string id, [Bind("Cognome,Nome,Mail,Password,Sesso,Nascita,Residenza")] Utente utente)
        //        {
        //            if (id != utente.Email)
        //            {
        //                return NotFound();
        //            }

        //            if (ModelState.IsValid)
        //            {
        //                try
        //                {
        //                    _context.Update(utente);
        //                    await _context.SaveChangesAsync();
        //                }
        //                catch (DbUpdateConcurrencyException)
        //                {
        //                    if (!UtenteExists(utente.Mail))
        //                    {
        //                        return NotFound();
        //                    }
        //                    else
        //                    {
        //                        throw;
        //                    }
        //                }
        //                return RedirectToAction(nameof(Index));
        //            }
        //            return View(utente);
        //        }

        //        // GET: Utente/Delete/5
        //        public async Task<IActionResult> Delete(string id)
        //        {
        //            if (id == null || _context.Utenti == null)
        //            {
        //                return NotFound();
        //            }

        //            var utente = await _context.Utenti
        //                .FirstOrDefaultAsync(m => m.Mail == id);
        //            if (utente == null)
        //            {
        //                return NotFound();
        //            }

        //            return View(utente);
        //        }

        //        // POST: Utente/Delete/5
        //        [HttpPost, ActionName("Delete")]
        //        [ValidateAntiForgeryToken]
        //        public async Task<IActionResult> DeleteConfirmed(string id)
        //        {
        //            if (_context.Utenti == null)
        //            {
        //                return Problem("Entity set 'AppDbContext.Utenti'  is null.");
        //            }
        //            var utente = await _context.Utenti.FindAsync(id);
        //            if (utente != null)
        //            {
        //                _context.Utenti.Remove(utente);
        //            }

        //            await _context.SaveChangesAsync();
        //            return RedirectToAction(nameof(Index));
        //        }

        //        private bool UtenteExists(string id)
        //        {
        //          return (_context.Utenti?.Any(e => e.Mail == id)).GetValueOrDefault();
        //        }
    }
}
