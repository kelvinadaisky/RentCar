using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentCar.Models;

namespace RentCar.Controllers
{
    public class AracController : Controller
    {
        private readonly RentCarContext _context;

        public AracController(RentCarContext context)
        {
            _context = context;
        }
        // GET: Arac
        public IActionResult Index()
        {
            var aracList = _context.Aracs.ToList();
            return View(aracList);
        }

        // GET: Arac/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Arac/Create
        [HttpPost]
        public IActionResult Create([Bind("IdAraba, Marka, Model, Renk, UretimYili, PlakaNumarasi, Durum")] Arac arac)
        {
            if (ModelState.IsValid)
            {
                _context.Add(arac);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(arac);
        }

        public IActionResult Edit(int id)
        {
            var arac = _context.Aracs.FirstOrDefault(a => a.IdAraba == id);
            if (arac == null)
            {
                return NotFound();
            }
            return View(arac);
        }

        [HttpPost]
        public IActionResult Edit(Arac arac)
        {
            if (ModelState.IsValid)
            {

                    _context.Update(arac);
                    _context.SaveChanges();

                TempData["success"] = "Car updated successfully";
                return RedirectToAction("Index");
            }
            return View(arac);
        }
        public IActionResult Delete(int id)
        {
            var arac = _context.Aracs.FirstOrDefault(a => a.IdAraba == id);
            if (arac == null)
            {
                return NotFound();
            }
            return View(arac);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var arac = _context.Aracs.FirstOrDefault(a => a.IdAraba == id);
            if (arac == null)
            {
                return NotFound();
            }

            _context.Aracs.Remove(arac);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
