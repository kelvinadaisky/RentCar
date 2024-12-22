using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        public IActionResult Index(string searchString)
        {
            var aracList = _context.Aracs.ToList();
            if (!string.IsNullOrEmpty(searchString))
            {
                aracList = aracList.Where(a => a.Marka.Contains(searchString) || a.Model.Contains(searchString) || a.PlakaNumarasi.Contains(searchString)).ToList();
            }
                return View(aracList);
        }

        public IActionResult Create()
        {
            ViewData["AjansList"] = _context.Ajans.Select(d => new SelectListItem
            {
                Value = d.IdAjans.ToString(),  
                Text = d.AjansAdi               
            }).ToList();

            var arac = new Arac();

            return View(arac);
        }

        // POST: Arac/Create
        [HttpPost]
        public IActionResult Create(Arac arac)
        {
            if (ModelState.IsValid)
            {
                _context.Add(arac);
                _context.SaveChanges();
                var aracDurum = new AracDurumu
                {
                    IdAraba = arac.IdAraba,  
                };

                _context.AracDurumus.Add(aracDurum);
                _context.SaveChanges();
                TempData["success"] = "Car created successfully";

                return RedirectToAction(nameof(Index));
            }
            ViewData["AjansList"] = _context.Ajans.Select(d => new SelectListItem
            {
                Value = d.IdAjans.ToString(),  
                Text = d.AjansAdi              
            }).ToList();
            return View(arac);
        }

        public IActionResult Edit(int id)
        {
            var arac = _context.Aracs.FirstOrDefault(a => a.IdAraba == id);
            if (arac == null)
            {
                return NotFound();
            }
            ViewData["AjansList"] = new SelectList(_context.Ajans, "IdAjans", "AjansAdi", arac.IdAjans);
            return View(arac);
        }

        [HttpPost]
        public IActionResult Edit(Arac arac)
        {
            if (ModelState.IsValid)
            {
                    _context.Update(arac);
                    _context.SaveChanges();

                TempData["success"] = "Car edited successfully";
                return RedirectToAction("Index");
            }
            ViewData["AjansList"] = new SelectList(_context.Ajans, "IdAjans", "AjansAdi", arac.IdAjans);
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
            TempData["success"] = "Car deleted successfully";

            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            var arac = _context.Aracs
                .Include(a => a.AracDurumu)    // Load related AracDurumu
                .Include(a => a.Bakim)        // Load related Bakim
                .Include(a => a.Sigortum)       // Load related Sigorta
                .Include(a => a.Sozlesmes)     // Load related Sozlesme
                .Include(a => a.IdAjansNavigation)
                .FirstOrDefault(a => a.IdAraba == id);

            if (arac == null)
            {
                return NotFound();
            }

            return View(arac);
        }

    }
}
