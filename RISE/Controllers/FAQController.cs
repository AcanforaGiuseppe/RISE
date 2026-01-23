/* RISE PROJECT - 2026 - COPYRIGHT by Acanfora Giuseppe */
using Microsoft.AspNetCore.Mvc;
using RISE.Data;

namespace RISE.Controllers
{
    public class FAQController : Controller
    {
        private readonly ApplicationDbContext _context;
        public FAQController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var faqs = _context.FAQs.ToList();
            return View(faqs);
        }

    }
}