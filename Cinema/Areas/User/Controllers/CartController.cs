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
using HtmlAgilityPack;
using Stripe.Checkout;

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

        public async Task<IActionResult> Remove(int cartId)
        {
            string utenteId = await GetCurrentUserId();
            var cart = _unitOfWork.ShoppingCart.GetFirst(cartId);
            if (cart != null)
            {
                _unitOfWork.ShoppingCart.Remove(cart);
                _unitOfWork.Save();
                HttpContext.Session.SetInt32(SD.SessionCart,
                            _unitOfWork.ShoppingCart.GetAll().Where(u => u.UtenteId == utenteId).Count());
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

                //verifica che la prenotazione non sia già nel carrello
                var listaCart = _unitOfWork.ShoppingCart.GetAll().Where(c => c.PrenotazioneId == prenotazioneId && c.UtenteId == s.UtenteId).ToList();

                if (listaCart.Count() == 0)
                {
                    //aggiunta nel carrello
                    _unitOfWork.ShoppingCart.Add(s);
                    _unitOfWork.Save();
                    HttpContext.Session.SetInt32(SD.SessionCart,
                            _unitOfWork.ShoppingCart.GetAll().Where(u => u.UtenteId == s.UtenteId).Count());
                }
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

                    //Stripe checkout
                    var host = HttpContext.Request.Host.Value;
                    var scheme = HttpContext.Request.Scheme;
                    var domain = scheme + Uri.SchemeDelimiter + host + "/";

                    var options = new SessionCreateOptions
                    {
                        //LineItems è la lista degli articoli che l'utente sta acquistando
                        LineItems = new List<SessionLineItemOptions>(),
                        Mode = "payment",
                        //se si specifica la modalità di pagamento verranno proposte solo le modalità specificate
                        //altrimenti verranno proposte tutte le modalità di pagamento previste dal sistema
                        //Url da richiamare se il pagamento ha avuto esito positivo
                        SuccessUrl = domain + $"User/Cart/Summary?id={ShoppingCartVM.OrderHeader.Id}",
                        //url da richiamare se il pagamento viene cancellato
                        CancelUrl = domain + $"User/Cart/index",
                    };

                    //configuro gli articoli che verranno pagati su Stripe
                    foreach (var item in ShoppingCartVM.ListCart)
                    {
                        var sessionLineItemOptions = new SessionLineItemOptions
                        {
                            PriceData = new SessionLineItemPriceDataOptions
                            {
                                //il prezzo deve essere convertito in long dopo essere stato moltiplicato per 100
                                //ad esempio, 20.5 diventa 2050
                                UnitAmount = (long)(item.Price * 100),
                                Currency = "eur",
                                ProductData = new SessionLineItemPriceDataProductDataOptions
                                {
                                    Name = item.PrenotazioneId.ToString(),
                                },
                            },
                            Quantity = 1,
                        };
                        options.LineItems.Add(sessionLineItemOptions);
                    }

                    //creo un SessionService per Stripe
                    var service = new SessionService();
                    Session session = service.Create(options);
                    _unitOfWork.OrderHeader.UpdateStripeSessionId(ShoppingCartVM.OrderHeader.Id, session.Id);
                    _unitOfWork.Save();

                    //effettuo la chimata a Stripe
                    Response.Headers.Add("Location", session.Url);


                    //metto le prenotazioni a pagato
                    foreach (var item in ShoppingCartVM.ListCart)
                    {
                        var prenotazioneFromDb = _unitOfWork.Prenotazione.GetFirstOrDefault(item.PrenotazioneId);
                        prenotazioneFromDb.Pagato = true;
                        _unitOfWork.Prenotazione.Update(prenotazioneFromDb);
                    }

                    //rimuovo gli articoli messi nell'ordine dalla ShoppingCart dell'utente
                    _unitOfWork.ShoppingCart.RemoveRange(ShoppingCartVM.ListCart);
                    _unitOfWork.Save();

                    HttpContext.Session.SetInt32(SD.SessionCart, 0);

                    return new StatusCodeResult(303);
                    //Fine del checkout su Stripe ****************

                    
                }
            }
            return RedirectToAction("Index", "Home");
        }

        public IActionResult OrderConfirmation(int id)
        {
            OrderHeader? orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(id);
            if (orderHeader != null)
            {
                //recupero la sessione di Stripe creata al checkout
                var service = new SessionService();
                Session session = service.Get(orderHeader.SessionId);
                //verifico lo stato del pagamento su Stripe
                if (session.PaymentStatus.ToLower() == "paid")
                {
                    //aggiorno lo stato di pagamento dell'ordine
                    _unitOfWork.OrderHeader.UpdateStatus(id, SD.StatusApproved, SD.PaymentStatusApproved);
                    //aggiorno il PaymentIntentId
                    _unitOfWork.OrderHeader.UpdateStripePaymentIntentId(id, session.PaymentIntentId);
                    //orderHeader.PaymentIntentId = session.PaymentIntentId;

                    //rimuovo gli articoli dalla ShopingCart dell'utente
                    List<ShoppingCart> shoppingCarts = _unitOfWork.ShoppingCart.GetAll().Where(u => u.UtenteId == orderHeader.UtenteId).ToList();
                    _unitOfWork.ShoppingCart.RemoveRange(shoppingCarts);
                    //rendo persistenti le modifiche nel database
                    _unitOfWork.Save();
                   
                    return View(id);
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