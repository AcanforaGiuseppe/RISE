using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RISE.Models
{
    public class Registration
    {
        public int Id { get; set; }

        [Required]
        public string ParticipantName { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        public string Country { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;

        [Required]
        public string Category { get; set; } = string.Empty;

        // ===== RELAZIONI =====

        [Required]
        public int CompetitionId { get; set; }

        [ForeignKey(nameof(CompetitionId))]
        public Competition? Competition { get; set; }   // ✅ nullable

        public int? UserId { get; set; }                  // ✅ nullable

        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }                   // ✅ nullable

        public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;
        public bool Approved { get; set; } = false;
    }
}
