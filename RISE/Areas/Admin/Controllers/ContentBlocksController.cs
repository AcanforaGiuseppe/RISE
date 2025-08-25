using Microsoft.AspNetCore.Mvc;
using RISE.Data;
using RISE.Models;

namespace RISE.Areas.Admin.Controllers
{
    public class ContentBlocksController : BaseAdminController
    {
        private readonly ApplicationDbContext _db;
        public ContentBlocksController(ApplicationDbContext db) { _db = db; }

        public IActionResult Index() => View(_db.ContentBlocks.OrderBy(b => b.Key).ToList());

        public IActionResult Create() => View(new ContentBlock());

        [HttpPost]
        public IActionResult Create(ContentBlock model)
        {
            if(!ModelState.IsValid) return View(model);
            _db.ContentBlocks.Add(model);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var b = _db.ContentBlocks.Find(id);
            if(b == null) return NotFound();
            return View(b);
        }

        [HttpPost]
        public IActionResult Edit(ContentBlock model)
        {
            if(!ModelState.IsValid) return View(model);
            _db.ContentBlocks.Update(model);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var b = _db.ContentBlocks.Find(id);
            if(b == null) return NotFound();
            _db.ContentBlocks.Remove(b);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
