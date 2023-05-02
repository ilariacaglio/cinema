using System;
using Cinema.DataAccess.Repository.IRepository;
using Cinema.Models;
using Cinema.Models.VM;
using Cinema.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Cinema.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class UtenteManagementController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public UtenteManagementController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult UtentiManagement()
        {
            return View();
        }

        //GET
        public IActionResult Edit(string userId)
        {
            UtenteVM u = new UtenteVM();
            u.ruoli = _unitOfWork.Utente.GetRoles().Select(s => new SelectListItem() {
                Text = s.Name,
                Value = s.Id
            });
            var utente = _unitOfWork.Utente.GetFirstOrDefaultUser(userId);
            if (utente != null)
            {
                u.user = utente;
                return View(u);
            }
            return RedirectToAction(nameof(UtentiManagement));
        }

        //edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(UtenteVM obj)
        {
            if (ModelState.IsValid)
            {
                if (obj != null)
                {
                    _unitOfWork.Utente.UpdateRuolo(obj.user.Id, obj.ruolo);
                }
                return RedirectToAction(nameof(UtentiManagement));
            }
            return View(obj);
        }

        [HttpGet]
        public IActionResult GetUtenti() {
            List<UtenteManagementVM> lista = new List<UtenteManagementVM>();
            var utenti = _unitOfWork.Utente.GetAllUsers();
            foreach (var item in utenti)
            {
                if (item.EmailConfirmed)
                {
                    lista.Add(new UtenteManagementVM()
                    {
                        UtenteId = item.Id,
                        Email = item.Email,
                        Ruolo = _unitOfWork.Utente.GetUserRole(item.Id).Name
                    });
                }
            }
            return Json(new { data = lista });
        }
    }
}