using Microsoft.AspNetCore.Mvc;
using RISE.Data;
using RISE.Models;

namespace RISE.Areas.Admin.Controllers
{
    public class FAQController : BaseAdminController
    {
        private readonly ApplicationDbContext _db;
        public FAQController(ApplicationDbContext db) { _db = db; }

        public IActionResult Index() => View(_db.FAQs.OrderBy(f => f.Id).ToList());

        public IActionResult Create() => View(new FAQ());

        [HttpPost]
        public IActionResult Create(FAQ model)
        {
            if(!ModelState.IsValid) return View(model);
            _db.FAQs.Add(model);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var item = _db.FAQs.Find(id);
            if(item == null) return NotFound();
            return View(item);
        }

        [HttpPost]
        public IActionResult Edit(FAQ model)
        {
            if(!ModelState.IsValid) return View(model);
            _db.FAQs.Update(model);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var item = _db.FAQs.Find(id);
            if(item == null) return NotFound();
            _db.FAQs.Remove(item);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
