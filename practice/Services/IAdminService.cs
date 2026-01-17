using practice.DTOs;
using practice.Models;

namespace practice.Services
{
    public interface IAdminService
    {
        public Task<DashboardStatsDto> GetDashboardStatsAsync();
        public Task<bool> ElectionTimeCheckerAsync();

        
        public Task<List<User>> GetAllUsersAsync();

        Task<bool> UpdateElectionAsync(Election election);
        Task<bool> DeleteElectionAsync(int id);
        public Task<List<Candidate>> GetAllCandidatesAsync();

        public Task<List<Election>> GetAllElectionsAsync();
        public Task<Election?> GetElectionByIdAsync(int id);
        public Task<bool> CreateElectionAsync(Election election);
        public Task<bool> ToggleElectionStatusAsync(int electionId);
        public Task<bool> PublishElectionResultsAsync(int electionId);
        public Task<int> RemoveCandidatefromElectionAsync(int candidateId);

        // Results (Moves that complex query out of Controller)
        public Task<List<VoteResultDto>> GetElectionResultsAsync(int electionId);
        public Task<List<User>> GetAllVotersAsync();
    }
}
