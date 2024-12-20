using Microsoft.AspNetCore.Mvc;
using RentCar.Models;
using RentCar.Utility;

namespace RentCar.Controllers
{
    public class OdemeController : Controller
    {
        private readonly RentCarContext _context;

        public OdemeController(RentCarContext context)
        {
            _context = context;

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


            TempData["SuccessMessage"] = "Ödeme başarıyla kaydedildi.";
            return RedirectToAction("Details", "Fatura", new { id = faturaId });
        }
    }
}
