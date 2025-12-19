using Microsoft.AspNetCore.Mvc;
using RISE.Data;
using RISE.Models.Admin;

namespace RISE.Areas.Admin.Controllers
{
    public class RetentionController : BaseAdminController
    {
        private readonly ApplicationDbContext _context;

        public RetentionController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var userRegs = _context.Registrations
                .GroupBy(r => r.UserId)
                .Select(g => g.Count())
                .ToList();

            var model = new UserRetentionViewModel
            {
                TotalUsers = userRegs.Count,
                OneTimeUsers = userRegs.Count(x => x == 1),
                ReturningUsers = userRegs.Count(x => x > 1)
            };

            return View(model);
        }
    }
}
