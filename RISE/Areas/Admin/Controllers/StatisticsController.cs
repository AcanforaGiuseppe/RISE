using Microsoft.AspNetCore.Mvc;
using RISE.Data;
using RISE.Models.Admin;

namespace RISE.Areas.Admin.Controllers
{
    public class StatisticsController : BaseAdminController
    {
        private readonly ApplicationDbContext _context;

        public StatisticsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var now = DateTime.UtcNow;
            var last7 = now.AddDays(-7);
            var last30 = now.AddDays(-30);

            var users = _context.Users.ToList();

            var model = new UserStatisticsViewModel
            {
                TotalUsers = users.Count,
                AdminUsers = users.Count(u => u.Role == "Admin"),
                NormalUsers = users.Count(u => u.Role == "User"),

                NewUsersLast7Days = users.Count(u => u.CreatedAt >= last7),
                NewUsersLast30Days = users.Count(u => u.CreatedAt >= last30),

                TotalRegistrations = _context.Registrations.Count(),
                TotalSubscribers = _context.NewsletterSubscribers.Count()
            };

            model.UsersPerDay = users
                .Where(u => u.CreatedAt >= last30)
                .GroupBy(u => u.CreatedAt.Date)
                .Select(g => new DailyStat
                {
                    Date = g.Key,
                    Count = g.Count()
                })
                .OrderBy(x => x.Date)
                .ToList();

            return View(model);
        }
    }
}
