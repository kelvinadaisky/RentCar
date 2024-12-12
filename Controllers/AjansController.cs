using Microsoft.AspNetCore.Mvc;
using RentCar.Models; // Adjust namespace based on your project structure
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

        // GET: AjansController
        public IActionResult Index()
        {
            var ajansList = _context.Ajans.ToList();
            return View(ajansList);
        }

        // GET: AjansController/Details/5
        public IActionResult Details(int id)
        {
            var ajan = _context.Ajans
                .FirstOrDefault(a => a.IdAjans == id);
            if (ajan == null)
            {
                return NotFound();
            }
            return View(ajan);
        }

        // GET: AjansController/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AjansController/Create
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

        // GET: AjansController/Edit/5
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

        // POST: AjansController/Edit/5
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

        // GET: AjansController/Delete/5
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

        // POST: AjansController/Delete/5
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
