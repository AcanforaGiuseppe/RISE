/* RISE PROJECT - 2026 - COPYRIGHT by Acanfora Giuseppe */
using Microsoft.AspNetCore.Mvc;
using RISE.Data;

namespace RISE.Areas.Admin.Controllers
{
    public class NewsletterController : BaseAdminController
    {
        private readonly ApplicationDbContext _db;
        public NewsletterController(ApplicationDbContext db) { _db = db; }
        public IActionResult Index() => View(_db.NewsletterSubscribers.OrderByDescending(s => s.SubscribedAt).ToList());
    }
}