using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentCar.Models;

namespace RentCar.Controllers
{
    public class FaturaController : Controller
    {
        private readonly RentCarContext _context;

        public FaturaController(RentCarContext context)
        {
            _context = context;
        }

        public IActionResult Details()
        {
            var faturas = _context.Faturas
                .Include(f => f.Odeme)
                .Include(f => f.IdSozlesmeNavigation)
                .ToList();


            return View(faturas);
        }

    }
}
