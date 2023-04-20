using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Cinema.Models;
using Cinema.DataAccess.Repository.IRepository;
using Cinema.Models.VM;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

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

