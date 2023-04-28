using Microsoft.AspNetCore.Mvc;
using Cinema.Models;
using Cinema.DataAccess.Repository.IRepository;
using Cinema.Models.VM;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Cinema.DataAccess.Repository;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Cinema.Utility;

namespace Cinema.Areas.User.Controllers
{
    [Area("User")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        [BindProperty]
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
                        ListCart = _unitOfWork.ShoppingCart.GetAll().Where(s => s.UtenteId == idUtente).ToList(),
                        OrderHeader = new()
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
                        ShoppingCartVM.OrderHeader.TotaleOrdine += cart.Price;
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

                //aggiunta nel carrello
                _unitOfWork.ShoppingCart.Add(s);
                _unitOfWork.Save();
            }
            
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Summary()
        {
            var userIdentity = User.Identity;
            string idUtente = await GetCurrentUserId();
            if (userIdentity != null)
            {
                var claimsIdentity = (ClaimsIdentity)userIdentity;
                var claim = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier);
                if (claim != null)
                {
                    ShoppingCartVM = new ShoppingCartVM()
                    {
                        //recupero i dati della ShoppingCart dal database
                        ListCart = _unitOfWork.ShoppingCart.GetAll().Where(s => s.UtenteId == idUtente).ToList(),
                        OrderHeader = new()
                    };
                    if (ShoppingCartVM.ListCart.Count() ==0)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    //recupero i dati dell'utente a partire dal suo Id --> claim.value corrisponde all'Id dell'utente in AspNetUsers
                    ShoppingCartVM.OrderHeader.Utente = _unitOfWork.Utente.GetFirstOrDefault(idUtente)!;
                    ShoppingCartVM.OrderHeader.Name = ShoppingCartVM.OrderHeader.Utente.Nome;
                    ShoppingCartVM.OrderHeader.PhoneNumber = ShoppingCartVM.OrderHeader.Utente.PhoneNumber ?? string.Empty;
                    ShoppingCartVM.OrderHeader.StreetAddress = ShoppingCartVM.OrderHeader.Utente.Residenza ?? string.Empty;
                    //calcolo il totale da mostrare nel summary
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
                        ShoppingCartVM.OrderHeader.TotaleOrdine += cart.Price;
                    }
                }
            }
            return View(ShoppingCartVM);
        }

        [HttpPost]
        [ActionName("Summary")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SummaryPOST()
        {
            var userIdentity = User.Identity;
            string idUtente = await GetCurrentUserId();
            if (userIdentity != null)
            {
                var claimsIdentity = (ClaimsIdentity)userIdentity;
                var claim = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier);
                if (claim != null)
                {
                    //definisco il contenuto dell'ordine
                    //recupero dal database i prodotti nella ShoppingCart
                    ShoppingCartVM.ListCart = _unitOfWork.ShoppingCart.GetAll().Where(s => s.UtenteId == idUtente).ToList();
                    //definisco i dati di OrderHeader
                    ShoppingCartVM.OrderHeader.DataOrdine = DateTime.Now;
                    ShoppingCartVM.OrderHeader.UtenteId = idUtente;
                    //calcolo il totale dell'ordine e lo salvo in OrderHeader.OrderTotal
                    //calcolo il totale da mostrare nel summary
                    foreach (var item in ShoppingCartVM.ListCart)
                    {
                        item.prenotazione = _unitOfWork.Prenotazione.GetFirstOrDefault(item.PrenotazioneId);

                        //calcola il prezzo di ogni prenotazione
                        var comprende = _unitOfWork.Comprende.GetAll().Where(c => c.IdPrenotazione == item.PrenotazioneId).ToList();
                        foreach (var obj in comprende)
                        {
                            var posto = _unitOfWork.Posto.GetFirstOrDefault(obj.IdPosto);
                            if (posto is not null)
                                item.Price += posto.Costo;
                        }
                        ShoppingCartVM.OrderHeader.TotaleOrdine += item.Price;
                    }

                    //salvo OrderHeader nel database -
                    //da questo momento in avanti ho l'Id di OrderHeader nel database che serve come FK in OrderDetail
                    _unitOfWork.OrderHeader.Add(ShoppingCartVM.OrderHeader);
                    _unitOfWork.Save();

                    //definisco i dati di OrderDetail
                    //ogni articolo nell'ordine, con relativa quantità e prezzo, corrisponde ad una riga nella tabella di OrderDetail
                    //ciascuna riga ha una FK su OrderId di OrderHeader
                    foreach (var cart in ShoppingCartVM.ListCart)
                    {
                        //definisco la riga in OrderDetail
                        OrderDetails orderDetail = new()
                        {
                            PrenotazioneId = cart.PrenotazioneId,
                            OrderId = ShoppingCartVM.OrderHeader.Id,
                            Price = cart.Price,
                        };
                        //salvo la riga nel database
                        _unitOfWork.OrderDetail.Add(orderDetail);
                        _unitOfWork.Save();
                    }
                    //rimuovo gli articoli messi nell'ordine dalla ShoppingCart dell'utente
                    _unitOfWork.ShoppingCart.RemoveRange(ShoppingCartVM.ListCart);
                    _unitOfWork.Save();
                }
            }
            return RedirectToAction("Index", "Home");
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