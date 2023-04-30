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
using Microsoft.Extensions.Hosting;

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

        [Authorize(Roles = SD.Role_Admin)]
        public IActionResult Index() {
            return View();
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

        [Authorize(Roles = SD.Role_User)]
        public IActionResult Details(int? id)
        {
            if (id == null || _unitOfWork.Prenotazione == null)
                return NotFound();

            var prenotazione = _unitOfWork.Prenotazione.GetFirstOrDefault(id);
            if (prenotazione == null)
                return NotFound();

            PrenotazioneDetailsVM p = new PrenotazioneDetailsVM();
            p.prenotazione = prenotazione;
            p.prenotazione.Spettacolo = _unitOfWork.Spettacolo.GetFirstOrDefault(prenotazione.DataS, prenotazione.OraS, prenotazione.IdSala);

            //postiPrenotati
            var comprende = _unitOfWork.Comprende.GetAll().Where(c => c.IdPrenotazione == id).ToList();
            foreach (var item in comprende)
            {
                var posto = _unitOfWork.Posto.GetFirstOrDefault(item.IdPosto);
                if (posto is not null)
                {
                    p.postiPrenotati.Add(posto);
                    p.costo += posto.Costo;
                }
            }

            //film
            p.imgFilm = _unitOfWork.Film.GetFirstOrDefault(p.prenotazione.Spettacolo.IdFilm).Img;

            return View(p);
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

                //vedi se per quella prenotazione ci sono già dei posti prenotati
                var prenotazioniPosto = _unitOfWork.Comprende.GetAll().Where(c => c.IdPrenotazione == obj.Id).ToList();

                //se ci sono aggiungi l'id in selezionati
                if (prenotazioniPosto.Count() != 0)
                {
                    foreach (var item in prenotazioniPosto)
                    {
                        var posti = _unitOfWork.Posto.GetAll().Where(p => p.Id == item.IdPosto).ToList();
                        foreach (var n in posti)
                            p.Selezionati.Add(n.Numero.ToString());
                    }
                }

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
                //controllo dei posti selezionati
                var posti = prenot.Selezionati.ElementAt(1).Replace("\"", "").Replace("[", "").Replace("]", "");
                if (posti.Equals("System.Collections.Generic.List`1System.String"))
                    return View(prenot);
                var stringArray = posti.Split(",");
                int[] numPosti = new int[stringArray.Length];
                for (int i = 0; i < stringArray.Length; i++)
                    numPosti[i] = int.Parse(stringArray[i]);

                if (prenot.p.Id == 0)
                {
                    //controllo che l'utente non superi i 4 posti che gli sono permessi
                    var prenotazioniPerUtente = _unitOfWork.Prenotazione.GetAll().Where(p => p.DataS == prenot.p.DataS && p.IdSala == prenot.p.IdSala && p.IdUtente == prenot.p.IdUtente && p.OraS == prenot.p.OraS).ToList();
                    if (prenotazioniPerUtente.Count() != 0)
                    {
                        //trova i posti
                        int num = 0;
                        foreach (var item in prenotazioniPerUtente)
                        {
                            var postiPerPrenotazione = _unitOfWork.Comprende.GetAll().Where(c => c.IdPrenotazione == item.Id).ToList();
                            num += postiPerPrenotazione.Count();
                        }
                        if (num != 0)
                        {
                            if (stringArray.Length + num > 4)
                                return View(prenot);
                        }
                    }

                    //aggiunta della prenotazione
                    _unitOfWork.Prenotazione.Add(prenot.p);
                    _unitOfWork.Save();
                    TempData["success"] = "Prenotazione creata con successo";

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
                }
                else
                {
                    if (DateTime.Now <= new DateTime(prenot.p.DataS.Year, prenot.p.DataS.Month, prenot.p.DataS.Day, prenot.p.OraS.AddHours(-1).Hour, prenot.p.OraS.Minute, 0, DateTimeKind.Local) && !prenot.p.Pagato)
                    {
                        //è possibile solo la modifica dei posti prenotati
                        //trova id posti
                        List<Posto> postiToDb = new List<Posto>();
                        foreach (var item in numPosti)
                        {
                            var query = _unitOfWork.Posto.GetAll().Where(p => p.Numero == item && p.IdSala == prenot.p.IdSala).ToList();
                            foreach (var result in query)
                                postiToDb.Add(result);
                        }

                        var postiFromDb = _unitOfWork.Comprende.GetAll().Where(c => c.IdPrenotazione == prenot.p.Id).ToList();

                        //controllo che l'utente non superi i 4 posti che gli sono permessi
                        var prenotazioniPerUtente = _unitOfWork.Prenotazione.GetAll().Where(p => p.DataS == prenot.p.DataS && p.IdSala == prenot.p.IdSala && p.IdUtente == prenot.p.IdUtente && p.OraS == prenot.p.OraS && p.Id != prenot.p.Id).ToList();
                        if (prenotazioniPerUtente.Count() != 0)
                        {
                            //trova i posti
                            int num = 0;
                            foreach (var item in prenotazioniPerUtente)
                            {
                                var postiPerPrenotazione = _unitOfWork.Comprende.GetAll().Where(c => c.IdPrenotazione == item.Id).ToList();
                                num += postiPerPrenotazione.Count();
                            }
                            if (num != 0)
                            {
                                if (postiToDb.Count()+ num > 4)
                                    return View(prenot);
                            }
                        }

                        //cancella comprende
                        foreach (var item in postiFromDb)
                        {
                            _unitOfWork.Comprende.Remove(item);
                            _unitOfWork.Save();
                        }

                        //scrivi comprende
                        foreach (var item in postiToDb)
                        {
                            _unitOfWork.Comprende.Add(new Comprende()
                            {
                                IdPosto = item.Id,
                                IdPrenotazione = prenot.p.Id
                            });
                            _unitOfWork.Save();
                        }
                    }
                    return RedirectToAction(nameof(IndexUtente));
                }
                //questo rimanda all'elenco delle prenotazioni dell'utente
                return RedirectToAction(nameof(IndexUtente));
            }
            return View(prenot);
        }

        public IActionResult Delete(int? id)
        {
            var objFromDbFirst = _unitOfWork.Prenotazione.GetFirstOrDefault(id);
            if (objFromDbFirst == null)
            {
                return RedirectToAction(nameof(IndexUtente));
            }
            else
            {
                _unitOfWork.Prenotazione.Remove(objFromDbFirst);
                _unitOfWork.Save();
                return RedirectToAction(nameof(IndexUtente));
            }
        }

        public IActionResult Edit(int prenotazioneId) {
            var prenotazione = _unitOfWork.Prenotazione.GetFirstOrDefault(prenotazioneId);
            if (prenotazione != null)
            {
                if (prenotazione.Pagato)
                    prenotazione.Pagato = false;
                else
                    prenotazione.Pagato = true;
                _unitOfWork.Prenotazione.Update(prenotazione);
                _unitOfWork.Save();
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Prenotazione
        [Authorize(Roles = SD.Role_Admin)]
        [HttpGet]
        public IActionResult GetAll()
        {
            List<PrenotazioneIndexVM> lista = new List<PrenotazioneIndexVM>();
            var prenotazioni = _unitOfWork.Prenotazione.GetAll();
            foreach (var item in prenotazioni)
            {
                //ricerca spettacolo
                var spettacolo = _unitOfWork.Spettacolo.GetFirstOrDefault(item.DataS, item.OraS, item.IdSala);

                //calcolo prezzo
                var comprende = _unitOfWork.Comprende.GetAll().Where(c => c.IdPrenotazione == item.Id).ToList();
                double prezzo = 0;
                foreach (var obj in comprende)
                    prezzo += _unitOfWork.Posto.GetFirstOrDefault(obj.IdPosto).Costo;

                lista.Add(new PrenotazioneIndexVM()
                {
                    id = item.Id,
                    dataS = item.DataS,
                    oraS = item.OraS,
                    idSala = item.IdSala,
                    pagato = item.Pagato,
                    emailUtente = _unitOfWork.Utente.GetFirstOrDefault(item.IdUtente).Email,
                    titoloFilm = _unitOfWork.Film.GetFirstOrDefault(spettacolo.IdFilm).Titolo,
                    prezzoTot = prezzo
                });
            }
            return Json(new { data = lista });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDbFirst = _unitOfWork.Prenotazione.GetFirstOrDefault(id);
            if (objFromDbFirst == null)
                return Json(new { success = false, message = "Error while deleting" });
            else
            {
                _unitOfWork.Prenotazione.Remove(objFromDbFirst);
                _unitOfWork.Save();
                return Json(new { success = true, message = "Delete Successful" });
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