namespace RISE.Models
{
    public class Registration
    {
        public int Id { get; set; }
        public string ParticipantName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int CompetitionId { get; set; }
        public Competition Competition { get; set; } = null!;
        public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;
        public bool Approved { get; set; } = false;
        public string Category { get; set; } = string.Empty;
    }
}