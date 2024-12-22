using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentCar.Models;
using System.Linq;

namespace RentCar.Controllers
{
    public class AjansController : Controller
    {
        private readonly RentCarContext _context;

        public AjansController(RentCarContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var ajansList = _context.Ajans.ToList();
            return View(ajansList);
        }


        public IActionResult Details(int id)
        {
            var ajan = _context.Ajans
                .Include(a => a.Aracs)
                .Include(a => a.Calisans)
                .FirstOrDefault(a => a.IdAjans == id);

            if (ajan == null)
            {
                return NotFound();
            }
            return View(ajan);
        }


 
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Ajan ajan)
        {
            if (ModelState.IsValid)
            {
                _context.Ajans.Add(ajan);
                _context.SaveChanges();
                TempData["success"] = "Agency created successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(ajan);
        }

        public IActionResult Edit(int id)
        {
            var ajan = _context.Ajans
                .FirstOrDefault(a => a.IdAjans == id);
            if (ajan == null)
            {
                return NotFound();
            }
            return View(ajan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Ajan ajan)
        {
            if (id != ajan.IdAjans)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                _context.Ajans.Update(ajan);
                _context.SaveChanges();
                TempData["success"] = "Agency updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(ajan);
        }

        public IActionResult Delete(int id)
        {
            var ajan = _context.Ajans
                .FirstOrDefault(a => a.IdAjans == id);
            if (ajan == null)
            {
                return NotFound();
            }
            return View(ajan);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var ajan = _context.Ajans
                .FirstOrDefault(a => a.IdAjans == id);
            if (ajan == null)
            {
                return NotFound();
            }

            _context.Ajans.Remove(ajan);
            _context.SaveChanges();
            TempData["success"] = "Agency deleted successfully!";
            return RedirectToAction(nameof(Index));
        }
    }
}
