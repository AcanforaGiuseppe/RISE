namespace RISE.Models
{
    public class Registration
    {
        public int Id { get; set; }

        // Participant info
        public string ParticipantName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        // Geography
        public string Country { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;

        // Competition relation
        public int CompetitionId { get; set; }
        public Competition Competition { get; set; } = null!;

        // User relation (IMPORTANT for retention & stats)
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        // Metadata
        public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;
        public bool Approved { get; set; } = false;
        public string Category { get; set; } = string.Empty;
    }
}
