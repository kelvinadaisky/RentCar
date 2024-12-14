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
        // GET: Arac
        public IActionResult Index()
        {
            var aracList = _context.Aracs.ToList();
            return View(aracList);
        }

        // GET: Arac/Create
        public IActionResult Create()
        {
            ViewData["AjansList"] = _context.Ajans.Select(d => new SelectListItem
            {
                Value = d.IdAjans.ToString(),  // Assuming IdAjans is the ID property
                Text = d.AjansAdi               // Assuming AjansAdi is the name property
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
                // Create an associated AracDurum for the new car
                var aracDurum = new AracDurumu
                {
                    IdAraba = arac.IdAraba,  // Assuming AracDurum has a foreign key IdAraba
                };

                _context.AracDurumus.Add(aracDurum);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AjansList"] = _context.Ajans.Select(d => new SelectListItem
            {
                Value = d.IdAjans.ToString(),  // Assuming IdAjans is the ID property
                Text = d.AjansAdi               // Assuming AjansAdi is the name property
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

                TempData["success"] = "Car updated successfully";
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
            return RedirectToAction("Index");
        }

        // GET: Arac/Details/5
        public IActionResult Details(int id)
        {
            var arac = _context.Aracs
                .Include(a => a.AracDurumu)    // Load related AracDurumu
                .Include(a => a.Bakims)        // Load related Bakim
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
