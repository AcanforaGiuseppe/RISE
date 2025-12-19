namespace RISE.Models.Admin
{
    public class UserStatisticsViewModel
    {
        public int TotalUsers { get; set; }
        public int AdminUsers { get; set; }
        public int NormalUsers { get; set; }

        public int NewUsersLast7Days { get; set; }
        public int NewUsersLast30Days { get; set; }

        public int TotalRegistrations { get; set; }
        public int TotalSubscribers { get; set; }

        public List<DailyStat> UsersPerDay { get; set; } = new();
    }

    public class DailyStat
    {
        public DateTime Date { get; set; }
        public int Count { get; set; }
    }
}
