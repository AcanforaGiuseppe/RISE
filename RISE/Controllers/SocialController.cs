/* RISE PROJECT - 2026 - COPYRIGHT by Acanfora Giuseppe */
using Microsoft.AspNetCore.Mvc;
using RISE.Data;

namespace RISE.Controllers
{
    public class SocialController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SocialController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var posts = _context.SocialPosts
                                .OrderByDescending(p => p.PostedAt)
                                .Take(12)
                                .ToList();

            return View(posts);
        }

    }
}