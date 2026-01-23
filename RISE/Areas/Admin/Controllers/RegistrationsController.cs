/* RISE PROJECT - 2026 - COPYRIGHT by Acanfora Giuseppe */
using Microsoft.AspNetCore.Mvc;
using RISE.Data;
using RISE.Security;

namespace RISE.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RegistrationsController : BaseAdminController
    {
        private readonly ApplicationDbContext _context;

        public RegistrationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // MOSTRA SOLO REGISTRAZIONI NON ANCORA APPROVATE
        public IActionResult Index()
        {
            var list = _context.Registrations
                    .Where(r => !r.Approved)
                    .OrderByDescending(r => r.RegisteredAt)
                    .ToList();

            return View(list);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Approve(int id)
        {
            var reg = _context.Registrations.FirstOrDefault(r => r.Id == id);
            if(reg == null) return NotFound();

            var email = reg.Email.Trim().ToLowerInvariant();

            var user = _context.Users.FirstOrDefault(u => u.Email == email);

            if(user == null)
            {
                user = new Models.User
                {
                    Email = reg.Email,
                    FullName = reg.ParticipantName,
                    Country = reg.Country,
                    City = reg.City,
                    Category = reg.Category,
                    PasswordHash = PasswordHasher.Hash("changeme"),
                    Role = "User",
                    CreatedAt = DateTime.Now
                };

                _context.Users.Add(user);
                _context.SaveChanges();
            }

            reg.UserId = user.Id;
            reg.Approved = true;

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Reject(int id)
        {
            var reg = _context.Registrations.Find(id);

            if(reg == null)
                return NotFound();

            _context.Registrations.Remove(reg);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

    }
}