using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using RISE.Data;
using RISE.Models;

namespace RISE.Controllers
{
    [Authorize(AuthenticationSchemes = "AdminCookie")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Dashboard()
        {
            return View();
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
    }
}