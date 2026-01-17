using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace practice.Models
{
    public class Candidate
    {
        [Key]
        public int Id { get; set; }
             

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

        [StringLength(500)]
        public string? ProfileImageUrl { get; set; }

        public int TotalVotes { get; set; } = 0;

        public bool IsApproved { get; set; } = false;

        public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;

        
        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual User User { get; set; } = null!;
        public virtual ICollection<Vote> Votes { get; set; } = new List<Vote>();

        [ForeignKey("Election")]
        public int? ElectionId { get; set; }  

        public virtual Election? Election { get; set; } 
    }
}
