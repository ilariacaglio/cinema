using Microsoft.AspNetCore.Mvc;
using Cinema.Models;
using Cinema.DataAccess.Repository.IRepository;
using Cinema.Models.VM;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Cinema.DataAccess.Repository;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace Cinema.Areas.User.Controllers
{
    [Area("User")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ShoppingCartVM ShoppingCartVM { get; set; }
        private readonly UserManager<Utente> _userManager;

        public CartController(IUnitOfWork unitOfWork, UserManager<Utente> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            ShoppingCartVM = new ShoppingCartVM();
        }

        public async Task<IActionResult> Index()
        {
            var userIdentity = User.Identity;
            if (userIdentity != null)
            {
                var claimsIdentity = (ClaimsIdentity)userIdentity;
                var claim = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier);
                string idUtente = await GetCurrentUserId();
                if (claim != null) {
                    ShoppingCartVM = new ShoppingCartVM()
                    {
                        ListCart = _unitOfWork.ShoppingCart.GetAll().Where(s => s.UtenteId == idUtente).ToList()
                    };
                    foreach (var item in ShoppingCartVM.ListCart)
                    {
                        item.prenotazione = _unitOfWork.Prenotazione.GetFirstOrDefault(item.PrenotazioneId);
                        item.dettagliPrenotazione = new PrenotazioneDetailsVM();
                        //calcola il prezzo di ogni prenotazione
                        var comprende = _unitOfWork.Comprende.GetAll().Where(c => c.IdPrenotazione == item.PrenotazioneId).ToList();
                        foreach (var obj in comprende)
                        {
                            var posto = _unitOfWork.Posto.GetFirstOrDefault(obj.IdPosto);
                            if (posto is not null)
                                item.Price += posto.Costo;
                        }

                        //immagine del film
                        item.prenotazione.Spettacolo = _unitOfWork.Spettacolo.GetFirstOrDefault(item.prenotazione.DataS, item.prenotazione.OraS, item.prenotazione.IdSala);
                        item.dettagliPrenotazione.imgFilm = _unitOfWork.Film.GetFirstOrDefault(item.prenotazione.Spettacolo.IdFilm).Img;
                    }
                    foreach (var cart in ShoppingCartVM.ListCart)
                    {
                        //calcola il prezzo totale
                        ShoppingCartVM.CartTotal += cart.Price;
                    }
                }
            }
            return View(ShoppingCartVM);
        }

        public IActionResult Remove(int cartId)
        {
            var cart = _unitOfWork.ShoppingCart.GetFirst(cartId);
            if (cart != null)
            {
                _unitOfWork.ShoppingCart.Remove(cart);
                _unitOfWork.Save();
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> AddToCart(int prenotazioneId)
        {
            if (prenotazioneId != 0)
            {
                ShoppingCart s = new ShoppingCart() {
                    PrenotazioneId = prenotazioneId,
                    prenotazione = _unitOfWork.Prenotazione.GetFirstOrDefault(prenotazioneId),
                    UtenteId = await GetCurrentUserId(),
                    dettagliPrenotazione = new PrenotazioneDetailsVM()
                };

                ////calcola il prezzo totale della prenotazione
                //var comprende = _unitOfWork.Comprende.GetAll().Where(c => c.IdPrenotazione == prenotazioneId).ToList();
                //foreach (var item in comprende)
                //{
                //    var posto = _unitOfWork.Posto.GetFirstOrDefault(item.IdPosto);
                //    if (posto is not null)
                //        s.Price += posto.Costo;
                //}

                ////immagine del film
                //s.prenotazione.Spettacolo = _unitOfWork.Spettacolo.GetFirstOrDefault(s.prenotazione.DataS, s.prenotazione.OraS, s.prenotazione.IdSala);
                //s.dettagliPrenotazione.imgFilm = _unitOfWork.Film.GetFirstOrDefault(s.prenotazione.Spettacolo.IdFilm).Img;

                //aggiunta nel carrello
                _unitOfWork.ShoppingCart.Add(s);
                _unitOfWork.Save();
            }
            
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