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
using Cinema.Models.VM;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Cinema.Controllers
{
    [Area("Admin")]
    public class ValutazioneController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ValutazioneController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: Valutazione
        public async Task<IActionResult> Index()
        {
            var valutazioni = _unitOfWork.Valutazione.GetAll();
            return View(valutazioni);
        }

        // GET: Valutazione/Details/5
        public async Task<IActionResult> Details(string idUtente, int? idFilm)
        {
            if (idUtente == null || idFilm == null || _unitOfWork.Valutazione == null)
                return NotFound();
            var valutazione = _unitOfWork.Valutazione.GetFirstOrDefault(idFilm, idUtente);

            if (valutazione == null)
                return NotFound();

            valutazione.IdFilmNavigation = _unitOfWork.Film.GetFirstOrDefault(valutazione.IdFilm);
            //sistema l'utente
            //valutazione.IdUtenteNavigation = _unitOfWork.


            return View(valutazione);
        }

        //GET
        public IActionResult Upsert(string idUtente, int? idFilm)
        {
            Valutazione v = new Valutazione();
            if (idFilm == 0)
                return View(v);
            else
            {
                var valutazioneInDb = _unitOfWork.Valutazione.GetFirstOrDefault(idFilm, idUtente);
                if (valutazioneInDb != null)
                {
                    v.IdFilm = valutazioneInDb.IdFilm;
                    v.IdFilmNavigation = _unitOfWork.Film.GetFirstOrDefault(v.IdFilm);
                    v.IdUtente = valutazioneInDb.IdUtente;
                    //fai l'iunitofwork
                    v.IdUtenteNavigation = _unitOfWork.Sala.GetFirstOrDefault(s.spettacolo.IdSala);
                    v.Voto = valutazioneInDb.Voto;
                    return View(s);
                }
                return View(s);
            }
        }

        //UPSERT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(SpettacoloVM obj)
        {
            if (ModelState.IsValid)
            {
                if (obj.prevData != obj.spettacolo.Data ||
                    obj.prevOra != obj.spettacolo.Ora ||
                    obj.prevSala != obj.spettacolo.IdSala)
                {
                    var spettacoloFromDb = _unitOfWork.Spettacolo.GetFirstOrDefault(obj.prevData, obj.prevOra, obj.prevSala);
                    if (spettacoloFromDb != null)
                        _unitOfWork.Spettacolo.Remove(spettacoloFromDb);
                }
                _unitOfWork.Spettacolo.Add(obj.spettacolo);
                TempData["success"] = "Spettacolo creato con successo";
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(obj);
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
