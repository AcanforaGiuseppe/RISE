using Microsoft.AspNetCore.Mvc;
using RISE.Data;
using RISE.Models;

namespace RISE.Areas.Admin.Controllers
{
    public class SocialController : BaseAdminController
    {
        private readonly ApplicationDbContext _context;

        public SocialController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.SocialPosts
                .OrderByDescending(x => x.PostedAt)
                .ToList());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(SocialPost post)
        {
            if(!ModelState.IsValid)
                return View(post);

            post.PostedAt = DateTime.Now;
            _context.SocialPosts.Add(post);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
