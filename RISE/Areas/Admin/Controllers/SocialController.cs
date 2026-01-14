using Microsoft.AspNetCore.Mvc;
using RISE.Data;
using RISE.Models;

namespace RISE.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SocialController : BaseAdminController
    {
        private readonly ApplicationDbContext _context;

        public SocialController(ApplicationDbContext context)
        {
            _context = context;
        }

        // LIST
        public IActionResult Index()
        {
            var posts = _context.SocialPosts
                .OrderByDescending(p => p.PostedAt)
                .ToList();

            return View(posts);
        }

        // CREATE
        public IActionResult Create()
            => View(new SocialPost());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(SocialPost model)
        {
            if(!ModelState.IsValid)
                return View(model);

            model.PostedAt = DateTime.UtcNow;

            _context.SocialPosts.Add(model);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // EDIT (GET)
        public IActionResult Edit(int id)
        {
            var post = _context.SocialPosts.Find(id);
            if(post == null) return NotFound();

            return View(post);
        }

        // EDIT (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(SocialPost model)
        {
            if(!ModelState.IsValid)
                return View(model);

            var post = _context.SocialPosts.Find(model.Id);
            if(post == null) return NotFound();

            post.Platform = model.Platform;
            post.Content = model.Content;
            post.ImageUrl = model.ImageUrl;
            post.ExternalUrl = model.ExternalUrl;

            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        // DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var post = _context.SocialPosts.Find(id);
            if(post == null) return NotFound();

            _context.SocialPosts.Remove(post);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
