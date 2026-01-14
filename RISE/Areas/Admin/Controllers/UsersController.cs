using Microsoft.AspNetCore.Mvc;
using RISE.Data;
using RISE.Models.Admin;

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

            // Retention "semplice": returning = utenti con >=2 registrations approvate
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

        // ========================= DETAILS =========================
        [HttpGet]
        public IActionResult Details(int id)
        {
            var user = _db.Users.FirstOrDefault(u => u.Id == id);
            if(user == null) return NotFound();
            return View(user);
        }

        // ========================= CREATE =========================
        [HttpGet]
        public IActionResult Create()
        {
            return View(new Models.User());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Models.User model)
        {
            // normalizzazione username/email
            model.Username = (model.Username ?? string.Empty).Trim().ToLowerInvariant();

            // blocco duplicati: 1 email = 1 user
            var exists = _db.Users.Any(u => (u.Username ?? "").Trim().ToLower() == model.Username);
            if(exists)
                ModelState.AddModelError(nameof(model.Username), "This email/username already exists.");

            if(!ModelState.IsValid)
                return View(model);

            model.PasswordHash = Security.PasswordHasher.Hash(model.PasswordHash);
            model.Role = string.IsNullOrWhiteSpace(model.Role) ? "User" : model.Role;

            _db.Users.Add(model);
            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // ========================= EDIT =========================
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var u = _db.Users.Find(id);
            if(u == null) return NotFound();
            return View(u);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Models.User model)
        {
            model.Username = (model.Username ?? string.Empty).Trim().ToLowerInvariant();

            // blocco duplicati su edit (escludendo se stesso)
            var exists = _db.Users.Any(u => u.Id != model.Id && (u.Username ?? "").Trim().ToLower() == model.Username);
            if(exists)
                ModelState.AddModelError(nameof(model.Username), "This email/username already exists.");

            if(!ModelState.IsValid)
                return View(model);

            var user = _db.Users.Find(model.Id);
            if(user == null) return NotFound();

            user.Username = model.Username;
            user.Role = model.Role;

            // aggiorna password SOLO se inserita
            if(!string.IsNullOrWhiteSpace(model.PasswordHash))
                user.PasswordHash = Security.PasswordHasher.Hash(model.PasswordHash);

            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        // ========================= DELETE =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var u = _db.Users.Find(id);
            if(u == null) return NotFound();

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