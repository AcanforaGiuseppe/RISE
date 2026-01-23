/* RISE PROJECT - 2026 - COPYRIGHT by Acanfora Giuseppe */
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RISE.Data;
using RISE.Models;
using RISE.Models.Admin;

namespace RISE.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin")]
    [Authorize(AuthenticationSchemes = "AdminCookie")]
    public class HomeController : BaseAdminController
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Route("Dashboard")]
        public IActionResult Dashboard()
        {
            var model = new AdminDashboardViewModel
            {
                NewsCount = _context.News.Count(),
                FAQCount = _context.FAQs.Count(),
                SubscribersCount = _context.NewsletterSubscribers.Count(),
                CompetitionsCount = _context.Competitions.Count(),
                RegistrationsCount = _context.Registrations.Count()
            };
            return View(model);
        }

        public IActionResult News()
        {
            return View(_context.News.OrderByDescending(n => n.PostedAt).ToList());
        }

        public IActionResult CreateNews()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateNews(News news)
        {
            if(ModelState.IsValid)
            {
                news.PostedAt = DateTime.Now;

                _context.News.Add(news);
                _context.SaveChanges();

                return RedirectToAction("News");
            }
            return View(news);
        }

        public IActionResult EditNews(int id)
        {
            var news = _context.News.Find(id);

            if(news == null)
                return NotFound();

            return View(news);
        }

        [HttpPost]
        public IActionResult EditNews(News updated)
        {
            var news = _context.News.Find(updated.Id);

            if(news == null)
                return NotFound();

            news.Title = updated.Title;
            news.Content = updated.Content;

            _context.SaveChanges();

            return RedirectToAction("News");
        }

        public IActionResult Subscribers()
        {
            var list = _context.NewsletterSubscribers
                               .OrderByDescending(x => x.SubscribedAt)
                               .ToList();
            return View(list);
        }

        public IActionResult Competitions()
        {
            var list = _context.Competitions.OrderByDescending(c => c.Date).ToList();
            return View(list);
        }

        public IActionResult CreateCompetition()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateCompetition(Competition competition)
        {
            if(ModelState.IsValid)
            {
                _context.Competitions.Add(competition);
                _context.SaveChanges();

                return RedirectToAction("Competitions");
            }
            return View(competition);
        }

        public IActionResult EditCompetition(int id)
        {
            var comp = _context.Competitions.Find(id);

            if(comp == null)
                return NotFound();

            return View(comp);
        }

        [HttpPost]
        public IActionResult EditCompetition(Competition updated)
        {
            var comp = _context.Competitions.Find(updated.Id);

            if(comp == null)
                return NotFound();

            comp.Title = updated.Title;
            comp.Location = updated.Location;
            comp.Date = updated.Date;
            comp.Description = updated.Description;
            comp.Results = updated.Results;

            _context.SaveChanges();

            return RedirectToAction("Competitions");
        }

        [HttpPost]
        public IActionResult DeleteCompetition(int id)
        {
            var comp = _context.Competitions.Find(id);

            if(comp == null)
                return NotFound();

            _context.Competitions.Remove(comp);
            _context.SaveChanges();
            return RedirectToAction("Competitions");
        }

        public IActionResult Registrations()
        {
            var list = _context.Registrations
                               .OrderByDescending(r => r.RegisteredAt)
                               .ToList();
            return View(list);
        }

        public IActionResult ApproveRegistration(int id)
        {
            var reg = _context.Registrations.Find(id);

            if(reg == null)
                return NotFound();

            reg.Approved = true;

            _context.SaveChanges();

            return RedirectToAction("Registrations");
        }

        [HttpPost]
        public IActionResult DeleteRegistration(int id)
        {
            var reg = _context.Registrations.Find(id);

            if(reg == null)
                return NotFound();

            _context.Registrations.Remove(reg);
            _context.SaveChanges();

            return RedirectToAction("Registrations");
        }

    }
}