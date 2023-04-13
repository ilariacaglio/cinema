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

namespace Cinema.Controllers
{
    [Area("Admin")]
    public class GenereController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public GenereController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: Genere
        public IActionResult Index()
        {
            var generi = _unitOfWork.Posto.GetAll();
            return View(generi);
        }

        // GET: Genere/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null || _unitOfWork.Genere == null)
                return NotFound();

            var genere = _unitOfWork.Genere.GetFirstOrDefault(id);
            if (genere == null)
                return NotFound();

            return View(genere);
        }

        //GET
        public IActionResult Upsert(int? id)
        {
            Genere g = new Genere();
            if (id == null || id == 0)
                return View(g);
            else
            {
                var genereInDb = _unitOfWork.Genere.GetFirstOrDefault(id);
                if (genereInDb != null)
                {
                    g = genereInDb;
                    return View(g);
                }
                return View(g);
            }
        }

        //UPSERT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Genere obj)
        {
            if (ModelState.IsValid)
            {
                if (obj.Id == 0)
                {
                    _unitOfWork.Genere.Add(obj);
                    TempData["success"] = "Posto creato con successo";
                }
                else 
                {
                    _unitOfWork.Genere.Update(obj);
                    TempData["success"] = "Posto modificato con successo";
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(obj);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var genereList = _unitOfWork.Genere.GetAll();
            return Json(new { data = genereList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var objFromDbFirst = _unitOfWork.Genere.GetFirstOrDefault(id);
            if (objFromDbFirst == null)//l'oggetto con l'id specificato non è stato trovato
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            else //l'oggetto con l'id specificato è stato trovato
            {
                _unitOfWork.Genere.Remove(objFromDbFirst);
                _unitOfWork.Save();
                return Json(new { success = true, message = "Delete Successful" });
            }
        }
    }
}
