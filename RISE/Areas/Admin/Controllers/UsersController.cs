/* RISE PROJECT - 2026 - COPYRIGHT by Acanfora Giuseppe */
using Microsoft.AspNetCore.Mvc;
using RISE.Data;
using RISE.Models;
using RISE.Models.Admin;
using RISE.Security;

namespace RISE.Areas.Admin.Controllers
{
    public class UsersController : BaseAdminController
    {
        private readonly ApplicationDbContext _db;

        public UsersController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var now = DateTime.UtcNow;

            var users = _db.Users
                     .OrderByDescending(u => u.CreatedAt)
                     .ToList();

            var approvedRegs = _db.Registrations.Where(r => r.Approved && r.UserId != null).ToList();
            var countsByUser = approvedRegs
                            .GroupBy(r => r.UserId!.Value)
                            .Select(g => g.Count())
                            .ToList();

            var returningUsers = countsByUser.Count(x => x > 1);

            var model = new UserDashboardViewModel
            {
                Users = users,
                TotalUsers = users.Count,
                NewUsersLast30Days = users.Count(u => u.CreatedAt >= now.AddDays(-30)),
                OldUsers = users.Count(u => u.CreatedAt < now.AddDays(-30))
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var user = _db.Users.FirstOrDefault(u => u.Id == id);

            if(user == null)
                return NotFound();

            return View(user);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new User());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(User model)
        {
            model.Email = (model.Email ?? "").Trim().ToLowerInvariant();
            model.FullName = model.FullName?.Trim();
            model.Country = model.Country?.Trim().ToLowerInvariant();
            model.City = model.City?.Trim();
            model.Category = model.Category?.Trim();

            // Blocco email duplicate
            if(_db.Users.Any(u => u.Email == model.Email))
                ModelState.AddModelError(nameof(model.Email), "This email already exists.");

            // Password obbligatoria SOLO per Admin
            if(model.Role == "Admin" && string.IsNullOrWhiteSpace(model.PasswordHash))
                ModelState.AddModelError(nameof(model.PasswordHash), "Password is required for Admin users.");

            if(!ModelState.IsValid)
                return View(model);

            // Hash password solo se presente
            if(!string.IsNullOrWhiteSpace(model.PasswordHash))
                model.PasswordHash = PasswordHasher.Hash(model.PasswordHash);
            else
                model.PasswordHash = string.Empty;

            model.CreatedAt = DateTime.Now;

            _db.Users.Add(model);
            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var user = _db.Users.Find(id);
            if(user == null) return NotFound();
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(User model)
        {
            model.Email = (model.Email ?? "").Trim().ToLowerInvariant();
            model.FullName = model.FullName?.Trim();
            model.Country = model.Country?.Trim().ToLowerInvariant();
            model.City = model.City?.Trim();
            model.Category = model.Category?.Trim();

            // Blocco email duplicate (escludendo se stesso)
            if(_db.Users.Any(u => u.Id != model.Id && u.Email == model.Email))
                ModelState.AddModelError(nameof(model.Email), "This email already exists.");

            // Password obbligatoria se ruolo Admin
            if(model.Role == "Admin" && string.IsNullOrWhiteSpace(model.PasswordHash))
                ModelState.AddModelError(nameof(model.PasswordHash), "Password is required for Admin users.");

            if(!ModelState.IsValid)
                return View(model);

            var user = _db.Users.Find(model.Id);
            if(user == null) return NotFound();

            user.Email = model.Email;
            user.FullName = model.FullName;
            user.Country = model.Country;
            user.City = model.City;
            user.Category = model.Category;
            user.Role = model.Role;

            // Aggiorna password solo se inserita
            if(!string.IsNullOrWhiteSpace(model.PasswordHash))
                user.PasswordHash = PasswordHasher.Hash(model.PasswordHash);

            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var u = _db.Users.Find(id);

            if(u == null)
                return NotFound();

            if(string.Equals(u.Role, "Admin", StringComparison.OrdinalIgnoreCase))
            {
                var admins = _db.Users.Count(x => x.Role == "Admin");
                if(admins <= 1)
                {
                    TempData["Error"] = "You cannot delete the last Admin user.";
                    return RedirectToAction(nameof(Index));
                }
            }

            _db.Users.Remove(u);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

    }
}