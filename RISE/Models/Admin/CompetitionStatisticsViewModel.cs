/* RISE PROJECT - 2026 - COPYRIGHT by Acanfora Giuseppe */
namespace RISE.Models.Admin
{
    public class CompetitionStatisticsViewModel
    {
        public int CompetitionId { get; set; }
        public string Title { get; set; } = string.Empty;
        public int TotalRegistrations { get; set; }
        public int ApprovedRegistrations { get; set; }
        public int PendingRegistrations { get; set; }
        public Dictionary<string, int> CategoryBreakdown { get; set; } = new();
        public List<DailyStat> RegistrationsPerDay { get; set; } = new();
    }
}