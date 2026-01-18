using System.ComponentModel.DataAnnotations;

namespace practice.DTOs
{
    public class VoteDto
    {
        [Required]
        public int ElectionId { get; set; }

        [Required]
        public int CandidateId { get; set; }
    }

    public class VoteResultDto
    {
        public int CandidateId { get; set; }
        public string CandidateName { get; set; } = string.Empty;
        public string PartyName { get; set; } = string.Empty;
        public string? PartySymbol { get; set; }
        public int TotalVotes { get; set; }
        public double VotePercentage { get; set; }
        public string PhotoUrl { get; set; }
    }

    public class CandidateDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string PartyName { get; set; } = string.Empty;
        public string? PartySymbol { get; set; }
        public string Manifesto { get; set; } = string.Empty;
        public string? Biography { get; set; }
        public string? Education { get; set; }
        public string? PreviousExperience { get; set; }
        public string? ProfileImageUrl { get; set; }
        public int TotalVotes { get; set; }
        public bool IsApproved { get; set; }

        public int? ElectionId { get; set; }
    }

    public class ElectionDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string ElectionType { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public bool ResultsPublished { get; set; }
        public int TotalVotes { get; set; }
    }
}
