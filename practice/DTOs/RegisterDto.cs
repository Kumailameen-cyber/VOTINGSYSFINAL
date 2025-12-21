using System.ComponentModel.DataAnnotations;

namespace practice.DTOs
{
    public class RegisterVoterDto
    {
        [Required]
        [StringLength(100)]
        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "Name can only contain letters and spaces")]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5})$",
    ErrorMessage = "Invalid email format. Use standard characters (a-z, 0-9, _, ., -) and a valid domain.")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        [Phone]
        [RegularExpression(@"^92\d{10}$", ErrorMessage = "Please enter a valid 10-digit Pakistan mobile number (starting with 9-2)")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        [StringLength(12)]
        [RegularExpression(@"^\d{12}$", ErrorMessage = "Aadhar Number must be exactly 12 digits")]
        public string AadharNumber { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string VoterIdNumber { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }

    public class RegisterCandidateDto
    {
        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        [Phone]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string AadharNumber { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string VoterIdNumber { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string PartyName { get; set; } = string.Empty;

        [StringLength(50)]
        public string? PartySymbol { get; set; }

        [Required]
        [StringLength(500)]
        public string Manifesto { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Biography { get; set; }

        [StringLength(200)]
        public string? Education { get; set; }

        [StringLength(200)]
        public string? PreviousExperience { get; set; }
    }
}
