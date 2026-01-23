/* RISE PROJECT - 2026 - COPYRIGHT by Acanfora Giuseppe */
using Microsoft.AspNetCore.Mvc;
using RISE.Data;

namespace RISE.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class GeographyController : BaseAdminController
    {
        private readonly ApplicationDbContext _context;

        public GeographyController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var data = _context.Users
                               .Where(u => !string.IsNullOrEmpty(u.Country))
                               .GroupBy(u => u.Country)
                               .Select(g => new
                               {
                                   Country = g.Key,
                                   Count = g.Count()
                               })
                               .OrderByDescending(x => x.Count)
                               .ToList();

            return View(data);
        }

    }
}