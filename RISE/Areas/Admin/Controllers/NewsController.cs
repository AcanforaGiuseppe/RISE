/* RISE PROJECT - 2026 - COPYRIGHT by Acanfora Giuseppe */
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

        public IActionResult Index()
        {
            var items = _db.News
                     .OrderByDescending(n => n.PostedAt)
                     .ToList();

            return View(items);
        }

        public IActionResult Create()
        {
            return View(new News());
        }

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

        public IActionResult Edit(int id)
        {
            var item = _db.News.Find(id);
            if(item == null)
                return NotFound();

            return View(item);
        }

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