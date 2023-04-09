using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Cinema.DataAccess;
using Cinema.Models;
using Cinema.DataAccess.Repository;
using Cinema.DataAccess.Repository.IRepository;

namespace Cinema.Controllers
{
    public class SalaController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public SalaController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: Sala
        public IActionResult Index()
        {
            var sale = _unitOfWork.Sala.GetAll();
            return View(sale);
        }

        // GET: Sala/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null || _unitOfWork.Sala == null)
                return NotFound();

            var sala = _unitOfWork.Sala.GetFirstOrDefault(id);

            if (sala == null)
                return NotFound();

            return View(sala);
        }

        //GET
        public IActionResult Upsert(int? id)
        {
            Sala s = new Sala();

            if (id == null || id == 0)
            {
                //create sala
                return View(s);
            }
            else
            {
                var salaInDb = _unitOfWork.Sala.GetFirstOrDefault(id);
                if (salaInDb != null)
                {
                    s = salaInDb;
                    return View(s);
                }
                return View(s);
            }
        }

        //UPSERT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Sala obj)
        {
            if (ModelState.IsValid)
            {
                if (obj.Id == 0)
                {
                    _unitOfWork.Sala.Add(obj);
                    TempData["success"] = "Sala creata con successo";
                }
                else //update exsisting Product
                {
                    _unitOfWork.Sala.Update(obj);
                    TempData["success"] = "Sala modificata con successo";
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(obj);
        }


        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var salaList = _unitOfWork.Sala.GetAll();
            return Json(new { data = salaList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var objFromDbFirst = _unitOfWork.Sala.GetFirstOrDefault(id);
            if (objFromDbFirst == null)//l'oggetto con l'id specificato non è stato trovato
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            else //l'oggetto con l'id specificato è stato trovato
            {
                _unitOfWork.Sala.Remove(objFromDbFirst);
                _unitOfWork.Save();
                return Json(new { success = true, message = "Delete Successful" });
            }
        }

        #endregion
    }
}