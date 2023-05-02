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
using Microsoft.AspNetCore.Identity;


namespace Cinema.Controllers
{
    [Area("User")]
    public class ValutazioneController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<Utente> _userManager;

        public ValutazioneController(IUnitOfWork unitOfWork, UserManager<Utente> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        // GET: Valutazione
        public async Task<IActionResult> Index()
        {
            string userId = await GetCurrentUserId();
            var valutazioni = _unitOfWork.Valutazione.GetAll().Where(v => v.IdUtente == userId).ToList();
            foreach (var item in valutazioni)
                item.IdFilmNavigation = _unitOfWork.Film.GetFirstOrDefault(item.IdFilm);
            return View(valutazioni);
        }

        //GET
        public async Task<IActionResult> Upsert(int idFilm)
        {
            string userId = await GetCurrentUserId();
                Valutazione v = new Valutazione()
                {
                    IdFilm = idFilm,
                    IdFilmNavigation = _unitOfWork.Film.GetFirstOrDefault(idFilm),
                    IdUtente = userId
                };
            if (idFilm == 0)
                return View(v);
            else
            {
                var valutazioneInDb = _unitOfWork.Valutazione.GetFirstOrDefault(idFilm, userId);
                if (valutazioneInDb != null)
                    return View(valutazioneInDb);
                return View(v);
            }
        }

        //UPSERT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpsertAsync(Valutazione obj)
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


        public async Task<IActionResult> Delete(int? idFilm)
        {
            string userId = await GetCurrentUserId();
            var objFromDbFirst = _unitOfWork.Valutazione.GetFirstOrDefault(idFilm, userId);
            if (objFromDbFirst == null)
                return RedirectToAction(nameof(Index));
            else
            {
                _unitOfWork.Valutazione.Remove(objFromDbFirst);
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public async Task<string> GetCurrentUserId()
        {
            Utente usr = await GetCurrentUserAsync();
            return usr?.Id;
        }
        private Task<Utente> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);
    }
}
