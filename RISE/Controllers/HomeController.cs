using Microsoft.AspNetCore.Mvc;
using RISE.Data;

namespace RISE.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var news = _context.News.OrderByDescending(n => n.PostedAt).Take(3).ToList();
            return View(news);
        }

        public IActionResult Competitions()
        {
            var comps = _context.Competitions.OrderByDescending(c => c.Date).ToList();
            return View(comps);
        }

        public IActionResult Rules()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }
    }
}