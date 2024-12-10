using RentCar.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace RentCar.Controllers
{
    public class KisiController : Controller
    {
        private readonly RentCarContext _context;

        public KisiController(RentCarContext context)
        {
            _context = context;
        }

        // GET: Kisi
        public async Task<IActionResult> Index(string roleFilter)
        {
            IQueryable<Kisi> query = _context.Kisis.AsQueryable();

            if (!string.IsNullOrEmpty(roleFilter))
            {
                query = query.Where(k => k.Role == roleFilter); // Filter by role
            }

            var kisiList = await query.ToListAsync();
            return View(kisiList);
        }
    }
}

