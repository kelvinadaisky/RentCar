using Microsoft.AspNetCore.Mvc;
using RentCar.Models;
using RentCar.Utility;

namespace RentCar.Controllers
{
    public class OdemeController : Controller
    {
        private readonly RentCarContext _context;
        private readonly SozlesmeService _sozlesmeService;

        public OdemeController(RentCarContext context, SozlesmeService sozlesmeService)
        {
            _context = context;
            _sozlesmeService = sozlesmeService;

        }

        // POST: Odeme/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(int faturaId, decimal tutar, string odemeYontemi)
        {
            var fatura = _context.Faturas.FirstOrDefault(f => f.IdFatura == faturaId);

            if (fatura == null)
            {
                TempData["ErrorMessage"] = "Fatura bulunamadı.";
                return RedirectToAction("Index", "Home");
            }

            var odeme = new Odeme
            {
                IdFatura = faturaId,
                Tutar = tutar,
                OdemeTarihi = DateOnly.FromDateTime(DateTime.Now),
                OdemeYontemi = odemeYontemi,
                OdemeDurumu = "Tamamlandi"
            };

            _context.Odemes.Add(odeme);
            _context.SaveChanges();

            // Use fatura.IdSozlesme instead of model.IdSozlesme
            if (fatura.IdSozlesme.HasValue)
            {
                _sozlesmeService.UpdateSozlesmeStatus(fatura.IdSozlesme.Value);
            }

            TempData["SuccessMessage"] = "Ödeme başarıyla kaydedildi.";
            return RedirectToAction("Details", "Fatura", new { id = faturaId });
        }
    }
}
