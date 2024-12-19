using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentCar.Models;

namespace RentCar.Controllers
{
    public class BakimController : Controller
    {
        private readonly RentCarContext _context;

        public BakimController(RentCarContext context)
        {
            _context = context;
        }

        // GET: Bakim/Create
        public IActionResult Create(int id)
        {
            var bakim = new Bakim { IdAraba = id };
            return View(bakim);
        }

        // POST: Bakim/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Bakim bakim)
        {
            if (ModelState.IsValid)
            {
                _context.Bakims.Add(bakim);
                _context.SaveChanges();
                TempData["success"] = "Maintenance record added successfully!";
                return RedirectToAction("Details", "Arac", new { id = bakim.IdAraba });
            }
            return View(bakim);
        }


        // GET: Bakim/Edit/5
        public IActionResult Edit(int id)
        {
            var bakim = _context.Bakims
                .Include(b => b.IdArabaNavigation) // Include related car data
                .FirstOrDefault(b => b.IdBakim == id);

            if (bakim == null)
            {
                return NotFound();
            }

            return View(bakim);
        }

        // POST: Bakim/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Bakim bakim)
        {
            if (id != bakim.IdBakim)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bakim);
                    _context.SaveChanges();
                    TempData["success"] = "Maintenance details updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Bakims.Any(b => b.IdBakim == id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction("Details", "Arac", new { id = bakim.IdAraba });
            }

            return View(bakim);
        }
    }
}
