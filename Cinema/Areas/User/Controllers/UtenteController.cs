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
    }
}
