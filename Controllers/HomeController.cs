using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentCar.Models;

namespace RentCar.Controllers
{

    public class HomeController : Controller
    {
        private readonly RentCarContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger , RentCarContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var totalCars = await _context.Set<TotalCarsCount>()
                                           .FromSqlRaw("SELECT GetTotalCars() AS Count")
                                           .ToListAsync();
            var totalCustomers = await _context.Set<TotalCustomersCount>()
                                    .FromSqlRaw("SELECT GetTotalCustomers() AS Count")
                                    .ToListAsync();

            var activeContracts = await _context.Set<StatsView>()
                                                .FromSqlRaw("SELECT getactivecontracts() AS Count")
                                                .ToListAsync();

            var monthlyRevenue = await _context.Set<MonthlyRevenue>()
                                    .FromSqlRaw("SELECT GetMonthlyRevenue() AS Revenue")
                                    .ToListAsync();
             
            int contractsCount = activeContracts.FirstOrDefault()?.Count ?? 0;
            int carsCount = totalCars.FirstOrDefault()?.Count ?? 0;
            int customersCount = totalCustomers.FirstOrDefault()?.Count ?? 0;
            decimal revenueAmount = monthlyRevenue.FirstOrDefault()?.Revenue ?? 0m; // Use 0m for decimal


            ViewBag.TotalCars = carsCount;
            ViewBag.TotalCustomers = customersCount;
            ViewBag.ActiveContracts = contractsCount;
            ViewBag.MonthlyRevenue = revenueAmount; 
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
