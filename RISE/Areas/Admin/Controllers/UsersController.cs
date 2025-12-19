using Microsoft.AspNetCore.Mvc;
using RISE.Data;
using RISE.Models;

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
            return View(_db.Users.ToList());
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
            if(!ModelState.IsValid)
                return View(model);

            model.PasswordHash = Security.PasswordHasher.Hash(model.PasswordHash);
            model.Role = string.IsNullOrWhiteSpace(model.Role) ? "User" : model.Role;

            _db.Users.Add(model);
            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var u = _db.Users.Find(id);
            if(u == null) return NotFound();
            return View(u);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(User model)
        {
            if(!ModelState.IsValid)
                return View(model);

            var user = _db.Users.Find(model.Id);
            if(user == null) return NotFound();

            user.Username = model.Username;
            user.Role = model.Role;

            // aggiorna password SOLO se inserita
            if(!string.IsNullOrWhiteSpace(model.PasswordHash))
            {
                user.PasswordHash = Security.PasswordHasher.Hash(model.PasswordHash);
            }

            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var u = _db.Users.Find(id);
            if(u == null) return NotFound();

            // Optional safety: do not allow removing the last Admin
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
