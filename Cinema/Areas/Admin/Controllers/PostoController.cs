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
using Cinema.Utility;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace Cinema.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class PostoController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public PostoController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: Posto
        public IActionResult Index()
        {
            var posti = _unitOfWork.Posto.GetAll();
            return View(posti);
        }

        // GET: Posto/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null || _unitOfWork.Posto == null)
                return NotFound();

            var posto = _unitOfWork.Posto.GetFirstOrDefault(id);

            if (posto == null)
                return NotFound();

            return View(posto);
        }


        //GET
        public IActionResult Upsert(int? id)
        {
            Posto p = new Posto();
            ViewBag.Sale = _unitOfWork.Sala.GetAll().Select(
                s => new SelectListItem
                {
                    Text = s.Id.ToString(),
                    Value = s.Id.ToString()
                });
            if (id == null || id == 0)
            {
                //create sala
                return View(p);
            }
            else
            {
                var postoInDb = _unitOfWork.Posto.GetFirstOrDefault(id);
                if (postoInDb != null)
                {
                    p = postoInDb;
                    return View(p);
                }
                return View(p);
            }
        }

        //UPSERT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Posto obj)
        {
            if (ModelState.IsValid)
            {
                if (obj.Id == 0)
                {
                    _unitOfWork.Posto.Add(obj);
                    TempData["success"] = "Posto creato con successo";
                }
                else //update exsisting Product
                {
                    _unitOfWork.Posto.Update(obj);
                    TempData["success"] = "Posto modificato con successo";
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
            var postoList = _unitOfWork.Posto.GetAll();
            return Json(new { data = postoList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var objFromDbFirst = _unitOfWork.Posto.GetFirstOrDefault(id);
            if (objFromDbFirst == null)//l'oggetto con l'id specificato non è stato trovato
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            else //l'oggetto con l'id specificato è stato trovato
            {
                _unitOfWork.Posto.Remove(objFromDbFirst);
                _unitOfWork.Save();
                return Json(new { success = true, message = "Delete Successful" });
            }
        }
        #endregion

    }
}
