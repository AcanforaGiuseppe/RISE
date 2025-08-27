using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using RISE.Data;
using RISE.Models;
using System.Linq;

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

        // GET: /Admin/Competitions
        public IActionResult Index()
        {
            var competitions = _context.Competitions
                                .OrderByDescending(c => c.Date)
                                .ToList();
            return View(competitions);
        }

        // GET: /Admin/Competitions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Admin/Competitions/Create
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

        // GET: /Admin/Competitions/Edit/5
        public IActionResult Edit(int id)
        {
            var competition = _context.Competitions.Find(id);
            if(competition == null) return NotFound();

            return View(competition);
        }

        // POST: /Admin/Competitions/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Competition updated)
        {
            var competition = _context.Competitions.Find(updated.Id);
            if(competition == null) return NotFound();

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

        // POST: /Admin/Competitions/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var competition = _context.Competitions.Find(id);
            if(competition == null) return NotFound();

            _context.Competitions.Remove(competition);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
