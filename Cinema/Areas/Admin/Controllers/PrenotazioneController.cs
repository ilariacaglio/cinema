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
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Cinema.Utility;

namespace Cinema.Controllers
{
    [Area("User")]
    public class PrenotazioneController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<Utente> _userManager;

        public PrenotazioneController(IUnitOfWork unitOfWork, UserManager<Utente> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        [Area("Admin")]
        // GET: Prenotazione
        [Authorize(Roles = SD.Role_Admin)]
        public IActionResult Index()
        {
            var prenotazioni = _unitOfWork.Prenotazione.GetAll();
            return View(prenotazioni);
        }

        // GET: Prenotazione
        [Authorize(Roles = SD.Role_User)]
        public async Task<IActionResult> IndexUtente()
        {
            string idUtente = await GetCurrentUserId();
            var prenotazioni = _unitOfWork.Prenotazione.GetAll().Where(p => p.IdUtente == idUtente);
            foreach (var item in prenotazioni)
            {
                item.Spettacolo = _unitOfWork.Spettacolo.GetFirstOrDefault(item.DataS, item.OraS, item.IdSala);
                item.Spettacolo.IdFilmNavigation = _unitOfWork.Film.GetFirstOrDefault(item.Spettacolo.IdFilm);
            }
            return View(prenotazioni);
        }

        // GET: Prenotazione/Details/5
        [Authorize(Roles = SD.Role_Admin)]
        [Authorize(Roles = SD.Role_User)]
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

        [Authorize(Roles = SD.Role_User)]
        public async Task<IActionResult> Upsert(DateOnly data, TimeOnly ora, int idSala, int? id )
        {
            PrenotazioneVM p = new PrenotazioneVM() {
                p = new Prenotazione() {
                    DataS = data,
                    OraS = ora,
                    IdSala = idSala,
                },
                postiSala = _unitOfWork.Sala.GetFirstOrDefault(idSala).Nposti,
                fileSala = _unitOfWork.Sala.GetFirstOrDefault(idSala).Nfile,
            };

            //trova posti liberi per quello spettacolo
            var prenotazioniPerSpettacolo = _unitOfWork.Prenotazione.GetAll().Where(p => p.DataS == data && p.OraS == ora && p.IdSala == idSala).ToList();
            if (prenotazioniPerSpettacolo.Count() != 0)
            {
                //conta i posti per ogni prenotazione
                foreach (var item in prenotazioniPerSpettacolo)
                {
                    var postiPrenotati = _unitOfWork.Comprende.GetAll().Where(c => c.IdPrenotazione == item.Id).ToList();
                    if (postiPrenotati.Count() != 0)
                    {
                        foreach (var posto in postiPrenotati) {
                            var seat = _unitOfWork.Posto.GetFirstOrDefault(posto.IdPosto);
                            p.Prenotati.Add(seat);
                        }
                    }
                }
            }

            //trova id utente
            p.p.IdUtente = await GetCurrentUserId();

            if (id == null || id == 0)
            {
                //create prenotazione
                return View(p);
            }
            else
            {
                var obj = _unitOfWork.Prenotazione.GetFirstOrDefault(id);
                if (obj != null)
                {
                    p.p = obj;
                    return View(p);
                }
                return View(p);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(PrenotazioneVM prenot) {
            if (ModelState.IsValid)
            {
                if (prenot.p.Id == 0)
                {
                    _unitOfWork.Prenotazione.Add(prenot.p);
                    _unitOfWork.Save();
                    TempData["success"] = "Prenotazione creata con successo";

                    //aggiunta dei posti selezionati
                    var posti = prenot.Selezionati.ElementAt(1).Replace("\"", "").Replace("[", "").Replace("]", "");
                    if (posti.Equals("System.Collections.Generic.List`1System.String"))
                        return View(prenot);
                    var stringArray = posti.Split(",");
                    int[] numPosti = new int[stringArray.Length];
                    for (int i = 0; i < stringArray.Length; i++)
                        numPosti[i] = int.Parse(stringArray[i]);

                    //trova id posti
                    List<Posto> postifromdb = new List<Posto>();
                    foreach (var item in numPosti)
                    {
                        var query = _unitOfWork.Posto.GetAll().Where(p => p.Numero == item && p.IdSala == prenot.p.IdSala).ToList();
                        foreach (var result in query)
                            postifromdb.Add(result);
                    }

                    //recupera l'id della prenotazione
                    var prenotazioneFromDb = _unitOfWork.Prenotazione.GetAll().Where(p => p.DataS == prenot.p.DataS && p.IdSala == prenot.p.IdSala && p.IdUtente == prenot.p.IdUtente && p.OraS == prenot.p.OraS).FirstOrDefault();

                    //scrivi comprende
                    foreach (var item in postifromdb)
                    {
                        _unitOfWork.Comprende.Add(new Comprende()
                        {
                            IdPosto = item.Id,
                            IdPrenotazione = prenotazioneFromDb.Id
                        });
                        _unitOfWork.Save();
                    }

                    //add to cart
                    //da implementare
                }
                else
                {

                }
                //questo rimanda all'elenco delle prenotazioni dell'utente
                return RedirectToAction(nameof(IndexUtente));
            }
            return View(prenot);
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

        [HttpGet]
        public async Task<string> GetCurrentUserId()
        {
            Utente usr = await GetCurrentUserAsync();
            return usr?.Id;
        }
        private Task<Utente> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);
    }
}
