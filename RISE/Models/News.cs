/* RISE PROJECT - 2026 - COPYRIGHT by Acanfora Giuseppe */
namespace RISE.Models
{
    public class News
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime PostedAt { get; set; } = DateTime.Now;
    }
}