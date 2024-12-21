using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

            try
            {
                _context.Odemes.Add(odeme);
                _context.SaveChanges();

                TempData["Success"] = "Ödeme başarıyla kaydedildi.";
                return RedirectToAction("Details", "Fatura", new { id = faturaId });
            }
            catch (DbUpdateException ex) when (ex.InnerException != null && ex.InnerException.Message.Contains("Payment amount must match the total invoice amount"))
            {
                // Handle specific Postgres exception
                TempData["Error"] = "Ödeme tutarı fatura tutarıyla eşleşmelidir.";
                return RedirectToAction("Details", "Fatura", new { id = faturaId });
            }
            catch (Exception)
            {
                // Handle other exceptions
                TempData["ErrorMessage"] = "Bir hata oluştu. Lütfen tekrar deneyin.";
                return RedirectToAction("Details", "Fatura", new { id = faturaId });
            }
        }
    }
}