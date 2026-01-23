/* RISE PROJECT - 2026 - COPYRIGHT by Acanfora Giuseppe */
using Microsoft.AspNetCore.Mvc;
using RISE.Data;
using RISE.Models;

namespace RISE.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class FAQController : BaseAdminController
    {
        private readonly ApplicationDbContext _context;

        public FAQController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index() => View(_context.FAQs.ToList());

        public IActionResult Create() => View(new FAQ());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(FAQ model)
        {
            if(!ModelState.IsValid)
                return View(model);

            _context.FAQs.Add(model);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var faq = _context.FAQs.Find(id);

            if(faq == null)
                return NotFound();

            return View(faq);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(FAQ model)
        {
            if(!ModelState.IsValid)
                return View(model);

            _context.FAQs.Update(model);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var faq = _context.FAQs.Find(id);

            if(faq == null)
                return NotFound();

            _context.FAQs.Remove(faq);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

    }
}