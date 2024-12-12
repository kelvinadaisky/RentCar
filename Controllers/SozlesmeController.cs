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
            ViewBag.AvailableCars = _context.Aracs.Where(a => a.Durum == true).ToList(); // Assuming 1 means available
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
        public IActionResult Index(int idMusteri, int idAraba, DateOnly imzalanmaTarihi, int sure, string kosullar)
        {
            ViewBag.AvailableCars = _context.Aracs.Where(a => a.Durum == true).ToList();
            ViewBag.Customers = _context.Kisis.ToList();
            var sozlesmes = _context.Sozlesmes.ToList();

            var musteri = _context.Musteris.Include(m => m.TcNavigation)
                .FirstOrDefault(m => m.Tc == idMusteri.ToString());
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
                ImzalanmaTarihi = imzalanmaTarihi,
                Sure = sure,
                Kosullar = kosullar,
                IdMusteri = musteri.Tc,
                IdAraba = arac.IdAraba
            };

            _context.Sozlesmes.Add(sozlesme);
            _context.SaveChanges();

            TempData["success"] = "Contract created successfully";
            return RedirectToAction("Index"); // Redirect to prevent form resubmission
        }


    }
}
