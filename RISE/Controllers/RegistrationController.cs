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
            // Navigation property non bindata dal form pubblico
            ModelState.Remove(nameof(model.Competition));

            // Normalizzo email (serve anche per controllo duplicati)
            var email = (model.Email ?? string.Empty).Trim().ToLowerInvariant();
            model.Email = email;

            if(!ModelState.IsValid)
                return View(model);

            // 1) Già approvato (presente come User)
            var alreadyApproved = _context.Users.Any(u =>
                (u.Email ?? "").Trim().ToLower() == email);

            // 2) Già in pending (registrazione già presente per la stessa competizione)
            var alreadyPending = _context.Registrations.Any(r =>
                (r.Email ?? "").Trim().ToLower() == email &&
                r.CompetitionId == model.CompetitionId);

            if(alreadyApproved || alreadyPending)
            {
                ModelState.AddModelError(nameof(model.Email), "This email is already registered.");
                return View(model);
            }

            // FIX DATI
            model.RegisteredAt = DateTime.UtcNow;
            model.Approved = false;

            // Normalizzazione utile per statistiche
            model.Country = model.Country?.Trim().ToLowerInvariant();
            model.City = model.City?.Trim();

            _context.Registrations.Add(model);
            _context.SaveChanges();

            // Popup via TempData (niente Success.cshtml)
            TempData["RegistrationSuccess"] = true;
            return RedirectToAction(nameof(Register));
        }
    }
}