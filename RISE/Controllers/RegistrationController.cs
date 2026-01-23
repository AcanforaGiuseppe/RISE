/* RISE PROJECT - 2026 - COPYRIGHT by Acanfora Giuseppe */
using Microsoft.AspNetCore.Mvc;
using RISE.Data;
using RISE.Models;

namespace RISE.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RegistrationController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View(new Registration());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(Registration model)
        {
            ModelState.Remove(nameof(model.Competition));

            var email = (model.Email ?? string.Empty).Trim().ToLowerInvariant();
            model.Email = email;

            if(!ModelState.IsValid)
                return View(model);

            var alreadyApproved = _context.Users.Any(u => (u.Email ?? "").Trim().ToLower() == email);

            var alreadyPending = _context.Registrations.Any(r => (r.Email ?? "").Trim().ToLower() == email && r.CompetitionId == model.CompetitionId);

            if(alreadyApproved || alreadyPending)
            {
                ModelState.AddModelError(nameof(model.Email), "This email is already registered.");
                return View(model);
            }

            model.RegisteredAt = DateTime.UtcNow;
            model.Approved = false;
            model.Country = model.Country?.Trim().ToLowerInvariant();
            model.City = model.City?.Trim();

            _context.Registrations.Add(model);
            _context.SaveChanges();

            TempData["RegistrationSuccess"] = true;
            return RedirectToAction(nameof(Register));
        }

    }
}