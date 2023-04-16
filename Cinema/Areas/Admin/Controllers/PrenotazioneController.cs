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
    public class PrenotazioneController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public PrenotazioneController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: Prenotazione
        public IActionResult Index()
        {
            var prenotazioni = _unitOfWork.Prenotazione.GetAll();
            return View(prenotazioni);
        }

        // GET: Prenotazione/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null || _unitOfWork.Prenotazione == null)
                return NotFound();

            var prenotazione = _unitOfWork.Prenotazione.GetFirstOrDefault(id);
            if (prenotazione == null)
                return NotFound();

            prenotazione.Spettacolo = _unitOfWork.Spettacolo.GetFirstOrDefault(prenotazione.DataS, prenotazione.OraS, prenotazione.IdSala);
            prenotazione.IdUtenteNavigation = _unitOfWork.Utente.GetFirstOrDefault(prenotazione.IdUtente);

            return View(prenotazione);
        }

        // GET: Prenotazione/Create
        //public IActionResult Create()
        //{
        //    ViewData["IdUtente"] = new SelectList(_context.Utenti, "Mail", "Mail");
        //    ViewData["DataS"] = new SelectList(_context.Spettacoli, "Data", "Data");
        //    return View();
        //}

        // POST: Prenotazione/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,DataS,OraS,IdSala,IdUtente")] Prenotazione prenotazione)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Prenotazione.Add(prenotazione);
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["IdUtente"] = new SelectList(_context.Utenti, "Mail", "Mail", prenotazione.IdUtente);
            //ViewData["DataS"] = new SelectList(_context.Spettacoli, "Data", "Data", prenotazione.DataS);
            return View(prenotazione);
        }


        [HttpGet]
        public IActionResult GetAll()
        {
            var prenotazioneList = _unitOfWork.Prenotazione.GetAll();
            return Json(new { data = prenotazioneList });
        }

        // POST: Prenotazione/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int? id)
        {
            if (_unitOfWork.Prenotazione == null || id == null)
                return Problem("Entity set 'AppDbContext.Prenotazioni'  is null.");
   
            var prenotazione = _unitOfWork.Prenotazione.GetFirstOrDefault(id);

            if (prenotazione != null)
                _unitOfWork.Prenotazione.Remove(prenotazione);

            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
    }
}
