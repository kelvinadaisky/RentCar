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

        public IActionResult Create()
        {
            var roleOptions = new List<SelectListItem>
            {
                new SelectListItem { Text = "Admin", Value = "Admin" },
                new SelectListItem { Text = "Musteri", Value = "Musteri" },
                new SelectListItem { Text = "Çalışan", Value = "Çalışan" }
            };

            ViewBag.RoleOptions = roleOptions;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Kisi kisi)
        {
            if (ModelState.IsValid)
            {
                if (!_context.Kisis.Any(k => k.Tc == kisi.Tc))
                {
                    _context.Kisis.Add(kisi);
                    _context.SaveChanges();
                }

                if (kisi.Role == "Musteri")
                {
                    return RedirectToAction("CreateMusteri", new { tc = kisi.Tc });
                }
                else if (kisi.Role == "Çalışan")
                {
                    return RedirectToAction("CreateCalisan", new { tc = kisi.Tc });
                }
                else if (kisi.Role == "Admin")
                {
                    return RedirectToAction("CreateAdmin", new { tc = kisi.Tc });
                }
            }

            return View(kisi);
        }


        public IActionResult CreateMusteri(string tc)
        {
            var musteri = new Musteri { Tc = tc };
            return View(musteri);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateMusteri(Musteri musteri, Ehliyet ehliyet)
        {
            ModelState.Remove("TcNavigation");
            ModelState.Remove("MusteriTcNavigation");
            ModelState.Remove("MusteriTc");
            if (ModelState.IsValid)
            {
                _context.Musteris.Add(musteri);
                ehliyet.MusteriTc = musteri.Tc;
                _context.Ehliyets.Add(ehliyet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(musteri);
        }

        public IActionResult CreateCalisan(string tc)
        {
            // Fetch the list of Ajans from the database
            var ajansList = _context.Ajans
                .Select(a => new SelectListItem
                {
                    Text = a.AjansAdi,
                    Value = a.IdAjans.ToString()
                })
                .ToList();

            // Pass the list to the view via ViewBag
            ViewBag.AjansList = ajansList;

            // Create a new Calisan object with the provided TC
            var calisan = new Calisan { Tc = tc };
            return View(calisan);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCalisan(Calisan calisan)
        {
            ModelState.Remove("TcNavigation");
            ModelState.Remove("IdAjansNavigation");
            if (ModelState.IsValid)
            {
                _context.Calisans.Add(calisan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var ajansList = _context.Ajans
                .Select(a => new SelectListItem
                {
                    Text = a.AjansAdi,
                    Value = a.IdAjans.ToString()
                })
                .ToList();

            // Pass the list to the view via ViewBag
            ViewBag.AjansList = ajansList;
            return View(calisan);
        }

        public IActionResult CreateAdmin(string tc)
        {
            var admin = new Admin { Tc = tc };
            return View(admin);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAdmin(Admin admin)
        {
            ModelState.Remove("TcNavigation");

            if (ModelState.IsValid)
            {
                _context.Admins.Add(admin);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(admin);
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
                new SelectListItem { Text = "Admin", Value = "Admin" },
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
