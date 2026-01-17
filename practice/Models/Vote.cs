using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace practice.Models
{
    public class Vote
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Election")]
        public int? ElectionId { get; set; }

        [ForeignKey("Voter")]
        public int VoterId { get; set; }

        [ForeignKey("Candidate")]
        public int CandidateId { get; set; }

        public DateTime VotedAt { get; set; } = DateTime.UtcNow;

        [StringLength(100)]
        public string? IpAddress { get; set; }

        public bool IsVerified { get; set; } = true;

        // Navigation properties
        public virtual Election Election { get; set; } = null!;
        public virtual User Voter { get; set; } = null!;
        public virtual Candidate Candidate { get; set; } = null!;
    }
}
