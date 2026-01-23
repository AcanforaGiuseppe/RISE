/* RISE PROJECT - 2026 - COPYRIGHT by Acanfora Giuseppe */
using Microsoft.AspNetCore.Mvc;
using RISE.Data;
using RISE.Models.Admin;

namespace RISE.Areas.Admin.Controllers
{
    public class CompetitionStatisticsController : BaseAdminController
    {
        private readonly ApplicationDbContext _context;

        public CompetitionStatisticsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Details(int id)
        {
            var competition = _context.Competitions.FirstOrDefault(c => c.Id == id);

            if(competition == null)
                return NotFound();

            var regs = _context.Registrations
                               .Where(r => r.CompetitionId == id)
                               .ToList();

            var model = new CompetitionStatisticsViewModel
            {
                CompetitionId = id,
                Title = competition.Title,
                TotalRegistrations = regs.Count,
                ApprovedRegistrations = regs.Count(r => r.Approved),
                PendingRegistrations = regs.Count(r => !r.Approved),
                CategoryBreakdown = regs
                                        .GroupBy(r => r.Category)
                                        .ToDictionary(g => g.Key, g => g.Count())
            };

            model.RegistrationsPerDay = regs
                                            .GroupBy(r => r.RegisteredAt.Date)
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