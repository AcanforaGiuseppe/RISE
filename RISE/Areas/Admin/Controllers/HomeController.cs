using Microsoft.AspNetCore.Mvc;
using RISE.Data;

namespace RISE.Areas.Admin.Controllers
{
    public class HomeController : BaseAdminController
    {
        private readonly ApplicationDbContext _db;
        public HomeController(ApplicationDbContext db) { _db = db; }

        public IActionResult Index()
        {
            ViewBag.NewsCount = _db.News.Count();
            ViewBag.FAQCount = _db.FAQs.Count();
            ViewBag.RegistrationsCount = _db.Registrations.Count();
            ViewBag.UsersCount = _db.Users.Count();
            ViewBag.SubscribersCount = _db.NewsletterSubscribers.Count();
            ViewBag.BlocksCount = _db.ContentBlocks.Count();
            return View();
        }
    }
}
