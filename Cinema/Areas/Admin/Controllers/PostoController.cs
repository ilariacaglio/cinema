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

namespace Cinema.Controllers
{
    public class PostoController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public PostoController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: Posto
        public IActionResult Index()
        {
            var posti = _unitOfWork.Posto.GetAll();
            return View(posti);
        }

        // GET: Posto/Details/5
        public IActionResult Details(int? id, int? idSala)
        {
            if (id == null || _unitOfWork.Posto == null)
                return NotFound();

            var posto = _unitOfWork.Posto.GetFirstOrDefault(id, idSala);

            if (posto == null)
                return NotFound();

            return View(posto);
        }

       
        //// POST: Posto/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    if (_context.Posti == null)
        //    {
        //        return Problem("Entity set 'AppDbContext.Posti'  is null.");
        //    }
        //    var posto = await _context.Posti.FindAsync(id);
        //    if (posto != null)
        //    {
        //        _context.Posti.Remove(posto);
        //    }
            
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool PostoExists(int id)
        //{
        //  return (_context.Posti?.Any(e => e.Id == id)).GetValueOrDefault();
        //}
    }
}
