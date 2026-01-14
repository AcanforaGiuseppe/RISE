namespace RISE.Models.Admin
{
    public class UserDashboardViewModel
    {
        public List<User> Users { get; set; } = new();
        public int TotalUsers { get; set; }
        public int NewUsersLast30Days { get; set; }
        public int OldUsers { get; set; }
        public int ReturningUsers => OldUsers;
        public double RetentionRate => TotalUsers == 0 ? 0 : (double)OldUsers / TotalUsers * 100;
    }
}