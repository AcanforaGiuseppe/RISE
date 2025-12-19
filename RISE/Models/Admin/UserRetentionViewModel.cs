namespace RISE.Models.Admin
{
    public class UserRetentionViewModel
    {
        public int TotalUsers { get; set; }
        public int OneTimeUsers { get; set; }
        public int ReturningUsers { get; set; }

        public double RetentionRate =>
            TotalUsers == 0 ? 0 : (double)ReturningUsers / TotalUsers * 100;
    }
}
