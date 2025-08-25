using Microsoft.AspNetCore.Mvc;
using RISE.Data;
using RISE.Models;

namespace RISE.Areas.Admin.Controllers
{
    public class NewsController : BaseAdminController
    {
        private readonly ApplicationDbContext _db;
        public NewsController(ApplicationDbContext db) { _db = db; }

        public IActionResult Index() => View(_db.News.OrderByDescending(n => n.PostedAt).ToList());

        public IActionResult Create() => View(new News());

        [HttpPost]
        public IActionResult Create(News model)
        {
            if(!ModelState.IsValid) return View(model);
            model.PostedAt = DateTime.UtcNow;
            _db.News.Add(model);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var item = _db.News.Find(id);
            if(item == null) return NotFound();
            return View(item);
        }

        [HttpPost]
        public IActionResult Edit(News model)
        {
            if(!ModelState.IsValid) return View(model);
            _db.News.Update(model);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var item = _db.News.Find(id);
            if(item == null) return NotFound();
            _db.News.Remove(item);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
