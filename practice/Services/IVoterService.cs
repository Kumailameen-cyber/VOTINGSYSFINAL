using practice.DTOs;
using practice.Models;

namespace practice.Services
{
    public interface IVoterService
    {
        public Task<VoterDashboardDto?> GetDashboardAsync(int userId);

        // Returns (Election, List of Candidates) or Null if invalid
        public Task<(Election? Election, List<CandidateDto> Candidates)> GetVotingPageAsync(int userId, int electionId);

        public Task<string> CastVoteAsync(int userId, VoteDto voteDto);
        public Task<bool> UpdateProfileAsync(UpdateProfileDto dto);
    }
}
