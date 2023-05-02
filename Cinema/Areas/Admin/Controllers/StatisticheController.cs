using System;
using System.IO;
using System.Security.Cryptography.Xml;
using Cinema.DataAccess.Repository.IRepository;
using Cinema.Models;
using Cinema.Models.VM;
using Cinema.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Cinema.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class StatisticheController : Controller
    {
        static HttpClient client = new HttpClient();
        private readonly IUnitOfWork _unitOfWork;

        public StatisticheController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult IncassiGiornalieri()
        {
            return View();
        }

        public IActionResult IncassoPerGiorni()
        {
            return View();
        }

        [HttpGet]
        public IActionResult JsonIncassiGiornalieri(DateOnly data)
        {
            List<IncassiGiornalieriVM> lista = CalcoloGuadagni(data);
            return Json(new { data = lista });
        }

        [HttpGet]
        public IActionResult JsonIncassoPerGiorni(DateOnly inizio, DateOnly fine) {
            List<IncassoPerGiorniVM> listaGiorni = new List<IncassoPerGiorniVM>();
            for (DateOnly data = inizio; data <= fine; data = data.AddDays(1))
            {
                List<IncassiGiornalieriVM> lista = CalcoloGuadagni(data);
               
                //calcolo guadagno complessivo
                IncassoPerGiorniVM _object = new IncassoPerGiorniVM() {
                    giorno = data
                };

                foreach (var item in lista)
                    _object.totaleGiorno += item.incasso;

                listaGiorni.Add(_object);
            }
            return Json(new { data = listaGiorni });
        }

        private List<IncassiGiornalieriVM> CalcoloGuadagni(DateOnly data) {
            var spettacoli = _unitOfWork.Spettacolo.GetAll().Where(s => s.Data == data).ToList();
            List<Prenotazione> listaP = new List<Prenotazione>();
            foreach (var item in spettacoli)
            {
                var prenotazioni = _unitOfWork.Prenotazione.GetAll().Where(p => p.DataS == item.Data && p.OraS == item.Ora && p.IdSala == item.IdSala).ToList();
                foreach (var obj in prenotazioni)
                {
                    obj.Spettacolo = item;
                    obj.Comprendes = _unitOfWork.Comprende.GetAll().Where(c => c.IdPrenotazione == obj.Id).ToList();
                    foreach (var o in obj.Comprendes)
                        o.Id = _unitOfWork.Posto.GetFirstOrDefault(o.IdPosto);
                    listaP.Add(obj);
                }
            }
            var a = listaP.GroupBy(i => i.Spettacolo.IdFilm, (key, g) => new { FilmId = key, Income = g.Sum(j => j.Comprendes.Sum(c => c.Id.Costo)) });

            List<IncassiGiornalieriVM> lista = new List<IncassiGiornalieriVM>();
            foreach (var item in a)
            {
                var film = _unitOfWork.Film.GetFirstOrDefault(item.FilmId);
                lista.Add(new IncassiGiornalieriVM()
                {
                    data = data,
                    titoloFilm = film.Titolo,
                    incasso = item.Income
                });
            }
            return lista;
        }
    }
}