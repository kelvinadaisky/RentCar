using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentCar.Models;

namespace RentCar.Controllers
{
    public class ReportsController : Controller
    {
        private readonly RentCarContext _context;

        public ReportsController(RentCarContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {

            ViewBag.RevenueByCar = await _context.GetRevenueByCar().ToListAsync();
            ViewBag.TopCustomers = await _context.GetTopCustomers(3).ToListAsync();
            ViewBag.MonthlyRevenueByAgence = await _context.GetMonthlyRevenueByAgence().ToListAsync();

            return View();
        }
    }
}
