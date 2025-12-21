using System.ComponentModel.DataAnnotations;

namespace practice.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "Name can only contain letters and spaces")]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(100)]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5})$",
    ErrorMessage = "Invalid email format. Use standard characters (a-z, 0-9, _, ., -) and a valid domain.")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        [RegularExpression(@"^92\d{10}$", ErrorMessage = "Please enter a valid 10-digit Pakistan mobile number (starting with 9-2)")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string Role { get; set; } = "Voter"; // Voter, Candidate, Admin

        [StringLength(12)]
        [RegularExpression(@"^\d{12}$", ErrorMessage = "Aadhar Number must be exactly 12 digits")]
        public string? AadharNumber { get; set; }

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
