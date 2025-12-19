using Microsoft.AspNetCore.Mvc;
using RISE.Data;
using RISE.Models;

namespace RISE.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public HomeController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Index()
        {
            // --- Slider images ---
            var sliderFolder = Path.Combine(_env.WebRootPath, "images", "slider");

            var images = Directory.Exists(sliderFolder)
                ? Directory.GetFiles(sliderFolder)
                    .Select(f => "/images/slider/" + Path.GetFileName(f))
                    .ToList()
                : new List<string>();

            // --- Latest news ---
            var news = _context.News
                .OrderByDescending(n => n.PostedAt)
                .Take(3)
                .ToList();

            var model = new HomeIndexViewModel
            {
                SliderImages = images,
                LatestNews = news
            };

            return View(model);
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

        public IActionResult Social()
        {
            var posts = _context.SocialPosts
                .OrderByDescending(x => x.PostedAt)
                .Take(6)
                .ToList();

            return View(posts);
        }

    }
}