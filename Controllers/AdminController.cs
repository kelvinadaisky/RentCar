using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentCar.Models;
using RentCar.Models.ViewModels;
using System.Security.Claims;

public class AdminController : Controller
{
    private readonly RentCarContext _context;

    public AdminController(RentCarContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        // Check credentials
        var admin = _context.Admins.Include(a => a.TcNavigation)
            .FirstOrDefault(a => a.Tc == model.Tc && a.Sifre == model.Sifre);

        if (admin == null)
        {
            TempData["error"] = "Invalid TC or Password";
            return View(model);
        }

        // Create claims
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, admin.TcNavigation.Ad),
            new Claim(ClaimTypes.Role, "Admin")
        };

        var claimsIdentity = new ClaimsIdentity(claims, "AdminCookie");
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        await HttpContext.SignInAsync("AdminCookie", claimsPrincipal);

        return RedirectToAction("Index", "Home");
    }

    [Authorize(Roles = "Admin")]
    public IActionResult Logout()
    {
        HttpContext.SignOutAsync("AdminCookie");
        return RedirectToAction("Login");
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public IActionResult AccessDenied()
    {
        return View();
    }
}
