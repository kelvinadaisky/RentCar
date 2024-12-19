using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentCar.Models;
using RentCar.Utility;

namespace RentCar.Controllers
{
    public class TeslimatController : Controller
    {
        private readonly RentCarContext _context;
        private readonly SozlesmeService _sozlesmeService;


        public TeslimatController(RentCarContext context , SozlesmeService sozlesmeService)
        {
            _context = context;
            _sozlesmeService = sozlesmeService;
        }
        // GET: Teslimat/Details
        [HttpGet]
        public IActionResult Details(int id)
        {
            var sozlesme = _context.Sozlesmes.FirstOrDefault(s => s.IdSozlesme == id);
            if (sozlesme == null)
            {
                TempData["ErrorMessage"] = "Sözleşme bulunamadı.";
                return RedirectToAction("Index", "Home"); // Redirect to your main page
            }

            var teslimatViewModel = new TeslimatViewModel
            {
                IdSozlesme = sozlesme.IdSozlesme,
                MusteriAdi = sozlesme.IdMusteri,
                //AracPlaka = sozlesme.AracPlaka
            };

            return View(teslimatViewModel);
        }

        // POST: Teslimat/Save
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save(TeslimatViewModel model)
        {
            // Step 1: Fetch Sözleşme
            var sozlesme = _context.Sozlesmes.FirstOrDefault(s => s.IdSozlesme == model.IdSozlesme);
            if (sozlesme == null)
            {
                TempData["ErrorMessage"] = "Sözleşme bulunamadı.";
                return RedirectToAction("Index", "Home");
            }

            // Step 2: Create Teslimat record
            var teslimat = new Teslimat
            {
                IdSozlesme = model.IdSozlesme,
                TeslimatTarihi = DateOnly.FromDateTime(DateTime.Now),
                Durum = "Teslim edildi"
            };
            _context.Teslimats.Add(teslimat);
            _context.SaveChanges();

            sozlesme.Durum = "Tamamlandi";
            _context.Sozlesmes.Update(sozlesme);
            _context.SaveChanges();

            // Step 3: Create HasarDurumu record
            var hasarDurumu = new HasarDurumu
            {
                IdTeslimat = teslimat.IdTeslimat, // Link to Teslimat
                Durumu = model.HasarDurumu
            };
            _context.HasarDurumus.Add(hasarDurumu);
            _context.SaveChanges();
            _sozlesmeService.UpdateSozlesmeStatus(model.IdSozlesme);
            TempData["SuccessMessage"] = "Teslimat ve Hasar Durumu başarıyla kaydedildi.";
            return RedirectToAction("Index", "Home");
        }

    }
}
