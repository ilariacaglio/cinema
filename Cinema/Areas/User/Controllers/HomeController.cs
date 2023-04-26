using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Cinema.Models;
using Cinema.DataAccess.Repository.IRepository;
using Cinema.Models.VM;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Cinema.DataAccess.Repository;
using System.Security.Claims;

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
            List<HomePageVM> lista = new List<HomePageVM>();
            string titolo = null;
            DateOnly data = new DateOnly();
            foreach (var item in film)
            {
                titolo = item.Titolo;
                var s = spettacoli.Find(s => s.IdFilm == item.Id);
                if (s != null)
                {
                    if (  DateTime.Today.AddDays(-3) <= DateTime.Parse(s.Data.ToString()).Date && DateTime.Today.AddDays(3) >= DateTime.Parse(s.Data.ToString()).Date)
                    {
                        data = s.Data;
                        lista.Add(new HomePageVM
                        {
                            Descrizione = item.Descrizione,
                            idFilm = item.Id,
                            NomeFilm = titolo,
                            Data = data,
                            Img = item.Img,
                            uscito = true
                        });
                    }
                    else if(DateTime.Today.AddDays(3) <= DateTime.Parse(s.Data.ToString()).Date)
                    {
                        data = s.Data;
                        lista.Add(new HomePageVM
                        {
                            Descrizione = item.Descrizione,
                            idFilm = item.Id,
                            NomeFilm = titolo,
                            Data = data,
                            Img = item.Img,
                            uscito = false
                        });
                    }
                }
            }

            return View(lista);
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
                    }
                    unitOfWork.Save();
                    RedirectToAction(nameof(Index));
                }
            }
        }
        return RedirectToAction(nameof(Index));
    }


    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

