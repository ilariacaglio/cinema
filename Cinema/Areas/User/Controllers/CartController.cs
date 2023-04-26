using Microsoft.AspNetCore.Mvc;
using Cinema.Models;
using Cinema.DataAccess.Repository.IRepository;
using Cinema.Models.VM;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Cinema.DataAccess.Repository;
using System.Security.Claims;

namespace Cinema.Areas.User.Controllers
{
    [Area("User")]
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}