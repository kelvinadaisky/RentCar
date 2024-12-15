using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentCar.Models;
using System.Linq;

namespace RentCar.Controllers
{
    public class SozlesmeController : Controller
    {
        private readonly RentCarContext _context;

        public SozlesmeController(RentCarContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewBag.AvailableCars = _context.Aracs
               .Include(a => a.AracDurumu)
               .Where(a => a.AracDurumu != null && a.AracDurumu.Aciklama == "Araç mevcut")
               .ToList();
            ViewBag.Customers = _context.Musteris.Include(m => m.TcNavigation).ToList();
            var sozlesmes = _context.Sozlesmes
                .Include(s => s.IdMusteriNavigation) // Load Musteri
                .ThenInclude(m => m.TcNavigation) // Load Kisi through Musteri
                .ToList();
            // Get the list of contracts
            return View(sozlesmes);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(string idMusteri, int idAraba, DateOnly imzalanmaTarihi, int sure, string kosullar, DateOnly cikisTarihi, DateOnly donusTarihi, string kiraSekli)
        {
            // Validation
            if (cikisTarihi > donusTarihi)
            {
                ModelState.AddModelError("", "Çıkış Tarihi dönüş tarihinden önce olmalıdır.");
                return RedirectToAction("Index");
            }

            // Calculate rental days
            int totalDays = (donusTarihi.ToDateTime(TimeOnly.MinValue) - cikisTarihi.ToDateTime(TimeOnly.MinValue)).Days;

            // Determine rental cost per day
            decimal costPerDay = kiraSekli switch
            {
                "Günlük" => 1000,
                "Haftalık" => 850,
                "Aylık" => 700,
                _ => 0
            };

            if (costPerDay == 0)
            {
                ModelState.AddModelError("", "Geçersiz kira şekli.");
                return RedirectToAction("Index");
            }

            decimal totalCost = totalDays * costPerDay;


            ViewBag.AvailableCars = _context.Aracs
              .Include(a => a.AracDurumu) // Change AracDurumus to AracDurumu
              .Where(a => a.AracDurumu != null && a.AracDurumu.Aciklama == "Araç mevcut")
              .ToList();
            ViewBag.Customers = _context.Musteris.Include(m => m.TcNavigation).ToList();
            var sozlesmes = _context.Sozlesmes.ToList();

            var musteri = _context.Musteris.Include(m => m.TcNavigation)
                .FirstOrDefault(m => m.Tc == idMusteri);

            if (musteri == null)
            {
                ModelState.AddModelError("", "Customer not found.");
                return View(sozlesmes);
            }

            var arac = _context.Aracs.FirstOrDefault(a => a.IdAraba == idAraba);
            if (arac == null)
            {
                ModelState.AddModelError("", "Car not found.");
                return View(sozlesmes);
            }

            Sozlesme sozlesme = new Sozlesme
            {
                IdMusteri = musteri.Tc,
                IdAraba = arac.IdAraba,
                CikisTarihi = cikisTarihi,
                DonusTarihi = donusTarihi,
                KiraSekli = kiraSekli,
                ToplamTutar = totalCost,
            };

            _context.Sozlesmes.Add(sozlesme);
            _context.SaveChanges();

            TempData["success"] = "Contract created successfully";
            return RedirectToAction("Index"); // Redirect to prevent form resubmission
        }

    }
}
