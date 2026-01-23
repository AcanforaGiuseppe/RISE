/* RISE PROJECT - 2026 - COPYRIGHT by Acanfora Giuseppe */
namespace RISE.Models
{
    public class SocialPost
    {
        public int Id { get; set; }
        public string Platform { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public string? ExternalUrl { get; set; }
        public DateTime PostedAt { get; set; } = DateTime.UtcNow;
    }
}