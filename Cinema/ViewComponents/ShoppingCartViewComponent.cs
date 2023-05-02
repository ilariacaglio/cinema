using System;
using System.Security.Claims;
using Cinema.DataAccess.Repository.IRepository;
using Cinema.Utility;
using Microsoft.AspNetCore.Mvc;

namespace Cinema.ViewComponents
{
	public class ShoppingCartViewComponent : ViewComponent
    {
        IUnitOfWork _unitOfWork;
        public ShoppingCartViewComponent(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IViewComponentResult Invoke() {
            var userIdentity = User.Identity;
            if (userIdentity != null)
            {
                var claimsIdentity = (ClaimsIdentity)userIdentity;
                var claim = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier);
                //se l'utente è signed in
                if (claim != null)
                {
                    //se la sessione contiene SD.SessionCart
                    if (HttpContext.Session.GetInt32(SD.SessionCart) != null)
                    {
                        //restituisco alla View di ViewComponent il valore della sessione per SD.SessionCart
                        return View(HttpContext.Session.GetInt32(SD.SessionCart));
                    }
                    else //la sessione non esiste oppure non contiene SD.SessionCart
                    {
                        //imposto SD.SessionCart nella sessione, recuperando il valore dal database
                        HttpContext.Session.SetInt32(SD.SessionCart,
                            _unitOfWork.ShoppingCart.GetAll().Where(u => u.UtenteId == claim.Value).ToList().Count);
                        //restituisco alla View di ViewComponent il valore della sessione per SD.SessionCart
                        return View(HttpContext.Session.GetInt32(SD.SessionCart));
                    }
                }
            }
            HttpContext.Session.Clear();
            return View(0);

        }
    }
}

