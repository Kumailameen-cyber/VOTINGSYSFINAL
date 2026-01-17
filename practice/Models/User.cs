using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace practice.Models
{
    [Index(nameof(Email), IsUnique = true)]
    [Index(nameof(cnic), IsUnique = true)]
    [Index(nameof(PhoneNumber), IsUnique = true)]
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        
        public string FullName { get; set; } = string.Empty;

        [Required]
        
        [EmailAddress]
        [StringLength(100)]
        
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string Role { get; set; } = "Voter"; // Voter, Candidate, Admin

        [StringLength(13)]
        [Required]

        
        public string cnic { get; set; } = string.Empty;

        [StringLength(20)]
        public string? VoterIdNumber { get; set; }

        public bool IsVerified { get; set; } = false;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public virtual ICollection<Vote> Votes { get; set; } = new List<Vote>();
        public virtual Candidate? CandidateProfile { get; set; }
    }
}
