using System.ComponentModel.DataAnnotations;

namespace practice.DTOs
{
    public class UpdateProfileDto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "Name can only contain letters and spaces")]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        [RegularExpression(@"^92\d{10}$", ErrorMessage = "Please enter a valid Pakistan mobile number (92XXXXXXXXXX)")]
        public string PhoneNumber { get; set; } = string.Empty;
    }
}