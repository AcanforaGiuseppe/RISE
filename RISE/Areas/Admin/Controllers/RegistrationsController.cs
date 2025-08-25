using Microsoft.AspNetCore.Mvc;
using RISE.Data;

namespace RISE.Areas.Admin.Controllers
{
    public class RegistrationsController : BaseAdminController
    {
        private readonly ApplicationDbContext _db;
        public RegistrationsController(ApplicationDbContext db) { _db = db; }

        public IActionResult Index() => View(_db.Registrations.OrderByDescending(r => r.RegisteredAt.ToString()).ToList());

        public IActionResult Delete(int id)
        {
            var r = _db.Registrations.Find(id);
            if(r == null) return NotFound();
            _db.Registrations.Remove(r);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
