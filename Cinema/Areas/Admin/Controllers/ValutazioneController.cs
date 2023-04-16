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
            valutazione.IdUtenteNavigation = _unitOfWork.Utente.GetFirstOrDefault(valutazione.IdUtente);
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
                    v.IdUtenteNavigation = _unitOfWork.Utente.GetFirstOrDefault(v.IdUtente);
                    return View(v);
                }
                return View(v);
            }
        }

        //UPSERT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Valutazione obj)
        {
            if (ModelState.IsValid)
            {
                var valutazioneInDb = _unitOfWork.Valutazione.GetFirstOrDefault(obj.IdFilm, obj.IdUtente);
                if (valutazioneInDb == null)
                {
                    _unitOfWork.Valutazione.Add(obj);
                }
                else
                {
                    valutazioneInDb.Voto = obj.Voto;
                    _unitOfWork.Valutazione.Update(valutazioneInDb);
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(obj);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var valutazioneList = _unitOfWork.Valutazione.GetAll();
            return Json(new { data = valutazioneList });
        }

        [HttpDelete]
        public IActionResult Delete(string idUtente, int? idFilm)
        {
            var objFromDbFirst = _unitOfWork.Valutazione.GetFirstOrDefault(idFilm, idUtente);
            if (objFromDbFirst == null)
                return Json(new { success = false, message = "Error while deleting" });
            else
            {
                _unitOfWork.Valutazione.Remove(objFromDbFirst);
                _unitOfWork.Save();
                return Json(new { success = true, message = "Delete Successful" });
            }
        }
    }
}
