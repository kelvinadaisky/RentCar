using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RentCar.Models;
using System.Linq;

namespace RentCar.Controllers
{
    public class AracDurumuController : Controller
    {
        private readonly RentCarContext _context;

        public AracDurumuController(RentCarContext context)
        {
            _context = context;
        }

        // GET: AracDurumu/Edit/5
        public IActionResult Edit(int id)
        {
            var aracDurumu = _context.AracDurumus
                .Include(ad => ad.IdArabaNavigation) // Include related car data
                .FirstOrDefault(ad => ad.IdDurum == id);

            if (aracDurumu == null)
            {
                return NotFound();
            }

            var options = new List<SelectListItem>
            {
                new SelectListItem { Text = "Araç mevcut", Value = "Araç mevcut" },
                new SelectListItem { Text = "Araç kiralanmış", Value = "Araç kiralanmış" },
                new SelectListItem { Text = "Araç bakımda", Value = "Araç bakımda" }
            };

            // Set the selected value
            ViewBag.StatusOptions = new SelectList(options, "Value", "Text", aracDurumu.Aciklama);

            return View(aracDurumu);
        }

        // POST: AracDurumu/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, AracDurumu aracDurumu)
        {
            if (id != aracDurumu.IdDurum)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    aracDurumu.GuncellemeTarihi = DateOnly.FromDateTime(DateTime.Now); // Update timestamp
                    _context.Update(aracDurumu);
                    _context.SaveChanges();
                    TempData["success"] = "Car status updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.AracDurumus.Any(ad => ad.IdDurum == id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction("Details", "Arac", new { id = aracDurumu.IdAraba });
            }

            return View(aracDurumu);
        }
    }
}
