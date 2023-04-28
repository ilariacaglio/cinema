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
using Cinema.DataAccess.Repository;
using Microsoft.Extensions.Hosting;
using Cinema.Models.VM;
using Cinema.Utility;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace Cinema.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class SpettacoloController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public SpettacoloController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: Spettacolo
        public IActionResult Index()
        {
            var spettacoli = _unitOfWork.Spettacolo.GetAll();
            return View(spettacoli);
        }

        // GET: Spettacolo/Details/5
        public async Task<IActionResult> Details(DateOnly data, TimeOnly ora, int? salaId)
        {
            if (salaId == null || _unitOfWork.Spettacolo == null)
                return NotFound();

            var spettacolo = _unitOfWork.Spettacolo.GetFirstOrDefault(data, ora, salaId);

            if (spettacolo == null)
                return NotFound();

            return View(spettacolo);
        }

        //GET
        public IActionResult Upsert(DateOnly data, TimeOnly ora, int? salaId)
        {
            SpettacoloVM s = new SpettacoloVM() {
                spettacolo = new Spettacolo(),
                SalaList = _unitOfWork.Sala.GetAll().Select(
                s => new SelectListItem
                {
                    Text = s.Id.ToString(),
                    Value = s.Id.ToString()
                }),
                FilmList = _unitOfWork.Film.GetAll().Select(
                f => new SelectListItem
                {
                    Text = f.Titolo,
                    Value = f.Id.ToString()
                })
            };
            if (salaId == 0)
                return View(s);
            else
            {
                var spettacoloInDb = _unitOfWork.Spettacolo.GetFirstOrDefault(data, ora, salaId);
                if (spettacoloInDb != null)
                {
                    s.spettacolo = spettacoloInDb;
                    s.spettacolo.IdFilmNavigation = _unitOfWork.Film.GetFirstOrDefault(s.spettacolo.IdFilm);
                    s.spettacolo.IdSalaNavigation = _unitOfWork.Sala.GetFirstOrDefault(s.spettacolo.IdSala);
                    s.prevData = spettacoloInDb.Data;
                    s.prevOra = spettacoloInDb.Ora;
                    s.prevSala = spettacoloInDb.IdSala;
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

        [HttpGet]
        public IActionResult GetAll()
        {
            List<SpettacoloIndexVM> lista = new List<SpettacoloIndexVM>();
            var spettacoli = _unitOfWork.Spettacolo.GetAll();
            foreach (var item in spettacoli)
            {
                lista.Add(new SpettacoloIndexVM
                {
                    Data = item.Data,
                    Ora = item.Ora,
                    salaId = item.IdSala,
                    Titolo = _unitOfWork.Film.GetFirstOrDefault(item.IdFilm).Titolo
                });
            }
            return Json(new { data = lista });
        }

        [HttpDelete]
        public IActionResult Delete(DateOnly data, TimeOnly ora, int? salaId)
        {
            var objFromDbFirst = _unitOfWork.Spettacolo.GetFirstOrDefault(data,ora ,salaId);
            if (objFromDbFirst == null)
                return Json(new { success = false, message = "Error while deleting" });
            else
            {
                _unitOfWork.Spettacolo.Remove(objFromDbFirst);
                _unitOfWork.Save();
                return Json(new { success = true, message = "Delete Successful" });
            }
        }
    }
}