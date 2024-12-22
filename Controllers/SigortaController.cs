    using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentCar.Models;

namespace RentCar.Controllers
{
    public class SigortaController : Controller
    {
        private readonly RentCarContext _context;

        public SigortaController(RentCarContext context)
        {
            _context = context;
        }


        public IActionResult Create(int id)
        {
            var araba = _context.Aracs.FirstOrDefault(a => a.IdAraba == id);

            if (araba == null)
            {
                return NotFound("Car not found");
            }

            var sigortum = new Sigortum
            {
                IdAraba = id,
            };

            return View(sigortum);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Sigortum sigortum)
        {
            if (_context.Aracs.Find(sigortum.IdAraba) == null)
            {
                ModelState.AddModelError("IdAraba", "Invalid car ID");
            }
            ModelState.Remove("IdArabaNavigation");

            if (ModelState.IsValid)
            {
                _context.Sigorta.Add(sigortum);
                _context.SaveChanges();
                TempData["success"] = "Insurance record added successfully!";
                return RedirectToAction("Details", "Arac", new { id = sigortum.IdAraba });
            }
            return View(sigortum);
        }


        public IActionResult Edit(int id)
        {
            var sigorta = _context.Sigorta
                .Include(s => s.IdArabaNavigation) 
                .FirstOrDefault(s => s.IdSigorta == id);

            if (sigorta == null)
            {
                return NotFound();
            }

            return View(sigorta);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Sigortum sigortum)
        {
            if (id != sigortum.IdSigorta)
            {
                return BadRequest();
            }
            ModelState.Remove("IdArabaNavigation");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sigortum);
                    _context.SaveChanges();
                    TempData["success"] = "Insurance details updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Sigorta.Any(s => s.IdSigorta == id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction("Details", "Arac", new { id = sigortum.IdAraba });
            }

            return View(sigortum);
        }
    }
}
