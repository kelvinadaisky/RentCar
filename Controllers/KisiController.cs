using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RentCar.Models;
using System.Threading.Tasks;

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

        // GET: Kisi/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kisi = await _context.Kisis
               .Include(k => k.Musteri)
               .ThenInclude(m => m.Ehliyet)
               .Include(k => k.Calisan)
               .ThenInclude(c => c.IdAjansNavigation)
               .FirstOrDefaultAsync(m => m.Tc == id);
            if (kisi == null)
            {
                return NotFound();
            }

            return View(kisi);
        }

        // GET: Kisi/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Kisi/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Tc,Ad,Soyad,Telefon,EPosta,Role")] Kisi kisi)
        {
            if (ModelState.IsValid)
            {
                _context.Add(kisi);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(kisi);
        }

        // GET: Kisi/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kisi = await _context.Kisis.FindAsync(id);
            if (kisi == null)
            {
                return NotFound();
            }
            var roleOptions = new List<SelectListItem>
            {
                //new SelectListItem { Text = "Admin", Value = "Admin" },
                new SelectListItem { Text = "Musteri", Value = "Musteri" },
                new SelectListItem { Text = "Çalışan", Value = "Çalışan" }
            };

            ViewBag.RoleOptions = roleOptions;
            return View(kisi);
        }

        // POST: Kisi/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Tc,Ad,Soyad,Telefon,EPosta,Role")] Kisi kisi)
        {
            if (id != kisi.Tc)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(kisi);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KisiExists(kisi.Tc))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(kisi);
        }

        // GET: Kisi/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kisi = await _context.Kisis.FirstOrDefaultAsync(m => m.Tc == id);
            if (kisi == null)
            {
                return NotFound();
            }

            return View(kisi);
        }

        // POST: Kisi/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var kisi = await _context.Kisis.FindAsync(id);
            if (kisi != null)
            {
                _context.Kisis.Remove(kisi);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool KisiExists(string id)
        {
            return _context.Kisis.Any(e => e.Tc == id);
        }
    }
}
