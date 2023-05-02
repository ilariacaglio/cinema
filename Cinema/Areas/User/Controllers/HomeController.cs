using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Cinema.Models;
using Cinema.DataAccess.Repository.IRepository;
using Cinema.Models.VM;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Cinema.DataAccess.Repository;
using System.Security.Claims;
using Cinema.Utility;

namespace Cinema.Controllers;
[Area("User")]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IUnitOfWork unitOfWork;

    public HomeController(ILogger<HomeController> logger, IUnitOfWork unit)
    {
        _logger = logger;
        unitOfWork = unit;
    }

    [AllowAnonymous]
    public IActionResult Index()
    {
        var film = unitOfWork.Film.GetAll().ToList();
        if (film != null) {
            var spettacoli = unitOfWork.Spettacolo.GetAll().ToList();
            //List<HomePageVM> lista = new List<HomePageVM>();
            HashSet<HomePageVM> lista = new HashSet<HomePageVM>();
            string titolo = null;
            DateOnly data = new DateOnly();
            foreach (var item in film)
            {
                titolo = item.Titolo;
                var s = spettacoli.Where(s => s.IdFilm == item.Id);
                //var s = spettacoli.Find(s => s.IdFilm == item.Id);
                foreach (var obj in s)
                {
                    if (obj != null)
                    {
                        if (DateTime.Today.AddDays(-3) <= DateTime.Parse(obj.Data.ToString()).Date && DateTime.Today.AddDays(3) >= DateTime.Parse(obj.Data.ToString()).Date)
                        {
                            data = obj.Data;
                            lista.Add(new HomePageVM
                            {
                                Descrizione = item.Descrizione,
                                idFilm = item.Id,
                                NomeFilm = titolo,
                                Data = data,
                                Img = item.Img,
                                uscito = true
                            });
                            break;
                        }
                        else if (DateTime.Today.AddDays(3) <= DateTime.Parse(obj.Data.ToString()).Date)
                        {
                            data = obj.Data;
                            lista.Add(new HomePageVM
                            {
                                Descrizione = item.Descrizione,
                                idFilm = item.Id,
                                NomeFilm = titolo,
                                Data = data,
                                Img = item.Img,
                                uscito = false
                            });
                            break;
                        }
                    }
                }
            }
            List<HomePageVM> homes = new List<HomePageVM>(lista);
            return View(homes);
        }
        return NotFound();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
    public IActionResult Details(ShoppingCart shoppingCart)
    {
        var userIdentity = User.Identity;
        if (userIdentity != null)
        {
            var claimsIdentity = (ClaimsIdentity)userIdentity;
            var claim = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier);
            if (claim != null)
            {
                //aggiorno il riferimento all'utente
                shoppingCart.UtenteId = claim.Value;

                //controllo che l'Id passato nel form corrisponda effettivamente a un prodotto nel database
                Prenotazione? selectedProductInDb = unitOfWork.Prenotazione.GetFirstOrDefault(shoppingCart.PrenotazioneId);
                if (selectedProductInDb != null)
                {
                    //verifico se c'è già un prodotto con lo stesso id nella shopping cart (nel database)
                    ShoppingCart? cartFromDb = unitOfWork.ShoppingCart.GetFirstOrDefault(shoppingCart.Id);

                    if (cartFromDb == null)//il prodotto non è presente nella shopping cart --> Add
                    {
                        //salvo shoppingCart: ha valori per ProductId, ApplicationUserId e Count. L'Id verrà definito dal database
                        unitOfWork.ShoppingCart.Add(shoppingCart);
                        unitOfWork.Save();
                    }
                    RedirectToAction(nameof(Index));
                }
            }
        }
        return RedirectToAction(nameof(Index));
    }

    [AllowAnonymous]
    public IActionResult Programmazione() {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [HttpGet]
    public IActionResult ProgrammazioneGet()
    {
        List<ProgrammazioneVM> lista = new List<ProgrammazioneVM>();

        //trova tutti i film
        var listaFilm = unitOfWork.Film.GetAll();
        foreach (var item in listaFilm)
        {
            var genere = unitOfWork.Genere.GetFirstOrDefault(item.IdGenere);
            lista.Add(new ProgrammazioneVM()
            {
                IdFilm = item.Id,
                TitoloFilm = item.Titolo,
                Genere = genere.Nome
            }) ;
        }

        //trova le date degli spettacoli
        foreach (var item in lista)
        {
            //date degli spettacoli
            var spettacoli = unitOfWork.Spettacolo.GetAll().Where(s => s.IdFilm == item.IdFilm).ToList();

            if (spettacoli.Count() != 0)
            {
                if (spettacoli.Count() == 1)
                {
                    item.DataInizio = spettacoli.FirstOrDefault().Data;
                    item.DataFine = item.DataInizio;
                }
                else
                {
                    //ordina elementi
                    spettacoli.Sort((s1, s2) => s1.Data.CompareTo(s2.Data));

                    //prendi il primo
                    item.DataInizio = spettacoli.FirstOrDefault().Data;

                    //prendi l'ultimo
                    item.DataFine = spettacoli.LastOrDefault().Data;
                }
            }
        }

        return Json(new { data = lista });
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

