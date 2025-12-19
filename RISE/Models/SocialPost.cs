namespace RISE.Models
{
    public class SocialPost
    {
        public int Id { get; set; }
        public string Platform { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public DateTime PostedAt { get; set; } = DateTime.Now;
    }
}
