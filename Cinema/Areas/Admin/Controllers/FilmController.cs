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
using Microsoft.Extensions.Hosting;

namespace Cinema.Controllers
{
    [Area("Admin")]
    public class FilmController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;

        public FilmController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }

        // GET: Film
        public IActionResult Index()
        {
            var film = _unitOfWork.Film.GetAll();
            return View(film);
        }

        // GET: Film/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null || _unitOfWork.Film == null)
                return NotFound();

            var film = _unitOfWork.Film.GetFirstOrDefault(id);
            
            if (film == null)
                return NotFound();
            film.IdGenereNavigation = _unitOfWork.Genere.GetFirstOrDefault(film.IdGenere);

            return View(film);
        }

        //GET
        public IActionResult Upsert(int? id)
        {
            Film f = new Film();
            ViewBag.Genere = _unitOfWork.Genere.GetAll().Select(
                g => new SelectListItem
                {
                    Text = g.Nome,
                    Value = g.Id.ToString()
                });
            if (id == null || id == 0)
                return View(f);
            else
            {
                var filmInDb = _unitOfWork.Film.GetFirstOrDefault(id);
                if (filmInDb != null)
                {
                    f = filmInDb;
                    f.IdGenereNavigation = _unitOfWork.Genere.GetFirstOrDefault(f.IdGenere);
                    return View(f);
                }
                return View(f);
            }
        }

        //UPSERT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Film obj, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploadDir = Path.Combine(wwwRootPath, "images", "film");
                    var fileExtension = Path.GetExtension(file.FileName);
                    if (obj.Img != null)
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, obj.Img.TrimStart(Path.DirectorySeparatorChar));
                        if (System.IO.File.Exists(oldImagePath))
                            System.IO.File.Delete(oldImagePath);
                    }
                    var filePath = Path.Combine(uploadDir, fileName + fileExtension);
                    var fileUrlString = filePath[wwwRootPath.Length..].Replace(@"\\", @"\");
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    obj.Img = fileUrlString;
                }
                if (obj.Id == 0)
                {
                    _unitOfWork.Film.Add(obj);
                    TempData["success"] = "Film creato con successo";
                }
                else
                {
                    _unitOfWork.Film.Update(obj);
                    TempData["success"] = "Film modificato con successo";
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(obj);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var filmList = _unitOfWork.Film.GetAll();
            return Json(new { data = filmList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var objFromDbFirst = _unitOfWork.Film.GetFirstOrDefault(id);
            if (objFromDbFirst == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            else 
            {
                if (objFromDbFirst.Img != null) //l'oggetto ha un ImageUrl
                {
                    var oldImagePath = Path.Combine(_hostEnvironment.WebRootPath, objFromDbFirst.Img.TrimStart(Path.DirectorySeparatorChar));
                    if (System.IO.File.Exists(oldImagePath))
                        System.IO.File.Delete(oldImagePath);
                }
                _unitOfWork.Film.Remove(objFromDbFirst);
                _unitOfWork.Save();
                return Json(new { success = true, message = "Delete Successful" });
            }
        }
    }
}