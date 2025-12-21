using System.ComponentModel.DataAnnotations;

namespace practice.Models
{
    public class Election
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        [StringLength(50)]
        public string ElectionType { get; set; } = "General"; // General, Local, etc.

        public bool IsActive { get; set; } = true;

        public bool ResultsPublished { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public virtual ICollection<Vote> Votes { get; set; } = new List<Vote>();
    }
}
