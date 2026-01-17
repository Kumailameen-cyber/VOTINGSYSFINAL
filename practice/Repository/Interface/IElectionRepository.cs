using practice.Data;
using practice.DTOs;
using practice.Models;
using practice.Services;

namespace practice.Repository.Interface
{
    public interface IElectionRepository
    {
        public Task<int> ActiveElectionAsync();
        public Task<bool> ElectionTimeCheckerAsync();

        public Task<int> TotalVotesCast();
      
        Task<bool> DeleteElectionAsync(int id);
        public Task<List<Election>> GetAllElectionsAsync();
        public Task<Election?> GetElectionByIdAsync(int id);
        public Task<bool> AddElectionAsync(Election election);
        public Task<bool> UpdateElectionAsync(Election election); // Used for Toggling & Publishing

        // Complex Query for Results
        public Task<List<VoteResultDto>> GetVoteResultsAsync(int electionId);

        public Task<List<Election>> GetOngoingElectionsAsync();
        public Task<Election?> GetLatestActiveOrPublishedElectionAsync();


        // Add these
        Task<List<int>> GetVotedElectionIdsAsync(int userId);
        Task<bool> HasUserVotedAsync(int electionId, int userId);
        Task<bool> CastVoteAsync(Vote vote, Candidate candidate); // Transactional Save
    }
}
