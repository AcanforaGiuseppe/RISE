/* RISE PROJECT - 2026 - COPYRIGHT by Acanfora Giuseppe */
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RISE.Data;
using RISE.Models;

namespace RISE.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = "AdminCookie")]
    public class CompetitionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CompetitionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var competitions = _context.Competitions
                                       .OrderByDescending(c => c.Date)
                                       .ToList();
            return View(competitions);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Competition competition)
        {
            if(ModelState.IsValid)
            {
                _context.Competitions.Add(competition);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            return View(competition);
        }

        public IActionResult Edit(int id)
        {
            var competition = _context.Competitions.Find(id);

            if(competition == null)
                return NotFound();

            return View(competition);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Competition updated)
        {
            var competition = _context.Competitions.Find(updated.Id);

            if(competition == null)
                return NotFound();

            if(ModelState.IsValid)
            {
                competition.Title = updated.Title;
                competition.Location = updated.Location;
                competition.Date = updated.Date;
                competition.Description = updated.Description;
                competition.Results = updated.Results;

                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            return View(updated);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var competition = _context.Competitions.Find(id);

            if(competition == null)
                return NotFound();

            _context.Competitions.Remove(competition);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

    }
}