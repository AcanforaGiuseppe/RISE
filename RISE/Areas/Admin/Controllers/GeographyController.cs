using Microsoft.AspNetCore.Mvc;
using RISE.Data;

namespace RISE.Areas.Admin.Controllers
{
    public class GeographyController : BaseAdminController
    {
        private readonly ApplicationDbContext _context;

        public GeographyController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var data = _context.Registrations
                .GroupBy(r => r.Country)
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
