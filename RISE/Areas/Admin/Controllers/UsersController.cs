using Microsoft.AspNetCore.Mvc;
using RISE.Data;
using RISE.Models;

namespace RISE.Areas.Admin.Controllers
{
    public class UsersController : BaseAdminController
    {
        private readonly ApplicationDbContext _db;
        public UsersController(ApplicationDbContext db) { _db = db; }

        public IActionResult Index() => View(_db.Users.ToList());

        public IActionResult Create() => View(new User());

        [HttpPost]
        public IActionResult Create(User model)
        {
            if(!ModelState.IsValid) return View(model);
            _db.Users.Add(model);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var u = _db.Users.Find(id);
            if(u == null) return NotFound();
            return View(u);
        }

        [HttpPost]
        public IActionResult Edit(User model)
        {
            if(!ModelState.IsValid) return View(model);
            _db.Users.Update(model);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var u = _db.Users.Find(id);
            if(u == null) return NotFound();
            _db.Users.Remove(u);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
