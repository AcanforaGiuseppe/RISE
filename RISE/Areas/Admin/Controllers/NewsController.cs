using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RISE.Data;
using RISE.Models;

namespace RISE.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = "AdminCookie")]
    public class NewsController : BaseAdminController
    {
        private readonly ApplicationDbContext _db;

        public NewsController(ApplicationDbContext db)
        {
            _db = db;
        }

        // LIST
        public IActionResult Index()
        {
            var items = _db.News
                .OrderByDescending(n => n.PostedAt)
                .ToList();

            return View(items);
        }

        // CREATE (GET)
        public IActionResult Create()
        {
            return View(new News());
        }

        // CREATE (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(News model)
        {
            if(!ModelState.IsValid)
                return View(model);

            model.PostedAt = DateTime.UtcNow;

            _db.News.Add(model);
            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // EDIT (GET)
        public IActionResult Edit(int id)
        {
            var item = _db.News.Find(id);
            if(item == null)
                return NotFound();

            return View(item);
        }

        // EDIT (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(News model)
        {
            if(!ModelState.IsValid)
                return View(model);

            _db.News.Update(model);
            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var item = _db.News.Find(id);
            if(item == null)
                return NotFound();

            _db.News.Remove(item);
            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
