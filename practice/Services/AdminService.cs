using Microsoft.EntityFrameworkCore;
using practice.Data; // Needed if we still use Context for Elections (or make an ElectionRepo)
using practice.DTOs;
using practice.Models;
using practice.Repository.Implementation;
using practice.Repository.Interface;

namespace practice.Services
{
    public class AdminService : IAdminService
    {
        private readonly IUserRepository _userRepo;
        private readonly ICandidateRepository _candidateRepo;
        private readonly IElectionRepository _electionRepo;

        public AdminService(IUserRepository userRepo, ICandidateRepository candidateRepo, IElectionRepository electionRepo)
        {
            _userRepo = userRepo;
            _candidateRepo = candidateRepo;
            _electionRepo = electionRepo;
        }

        public async Task<DashboardStatsDto> GetDashboardStatsAsync()
        {
            var stats = new DashboardStatsDto
            {
                // We use the Repository methods we created earlier
                TotalVoters = await _userRepo.GetTotalVotersCountAsync(),
                PendingVerifications = await _userRepo.GetPendingVerificationsCountAsync(),

                TotalCandidates = await _candidateRepo.GetTotalCandidatesCountAsync(),
                PendingApprovals = await _candidateRepo.GetPendingApprovalsCountAsync(),

                // Temporary: Direct Context for Elections (until you make ElectionRepo)
                ActiveElections = await _electionRepo.ActiveElectionAsync(),
                TotalVotesCast = await _electionRepo.TotalVotesCast(),

                RecentUsers = await _userRepo.GetRecentPendingUsersAsync(),
                RecentCandidates = await _candidateRepo.GetRecentPendingCandidatesAsync(),
            };

            return stats;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _userRepo.GetAllUsersExcludingAdminAsync();
        }
        public async Task<List<User>> GetAllVotersAsync()
        {
            return await _userRepo.GetAllUsersVotersAsync();
        }

        public async Task<List<Candidate>> GetAllCandidatesAsync()
        {
            return await _candidateRepo.GetAllCandidatesWithUsersAsync();
        }
        public async Task<List<Election>> GetAllElectionsAsync()
        {
            return await _electionRepo.GetAllElectionsAsync();
        }

        public async Task<Election?> GetElectionByIdAsync(int id)
        {
            return await _electionRepo.GetElectionByIdAsync(id);
        }

        public async Task<bool> CreateElectionAsync(Election election)
        {
            election.CreatedAt = DateTime.UtcNow;
            if (election.CreatedAt > election.EndDate) return false;
            return await _electionRepo.AddElectionAsync(election);
        }

        public async Task<bool> ToggleElectionStatusAsync(int electionId)
        {
            var election = await _electionRepo.GetElectionByIdAsync(electionId);
            if (election == null) return false;

            election.IsActive = !election.IsActive;
            election.UpdatedAt = DateTime.UtcNow;

            var candidates = await _candidateRepo.AllCandidateInElectionAsync(electionId);

            foreach (var candidate in candidates)
            {
                candidate.ElectionId = null;
                await _candidateRepo.UpdateCandidateAsync(candidate);
            }

            return await _electionRepo.UpdateElectionAsync(election);
        }

        public async Task<bool> PublishElectionResultsAsync(int electionId)
        {
            var election = await _electionRepo.GetElectionByIdAsync(electionId);
            if (election == null) return false;

            election.ResultsPublished = true;
            election.UpdatedAt = DateTime.UtcNow;
            return await _electionRepo.UpdateElectionAsync(election);
        }

        public async Task<List<VoteResultDto>> GetElectionResultsAsync(int electionId)
        {
            // This assumes you moved that complex query to ElectionRepository
            // If not, put the logic here calling _context or repositories
            return await _electionRepo.GetVoteResultsAsync(electionId);
        }

        public async Task<bool> UpdateElectionAsync(Election election)
        {
            return await _electionRepo.UpdateElectionAsync(election);
        }

        public async  Task<bool> DeleteElectionAsync(int id)
        {
            return await _electionRepo.DeleteElectionAsync(id);
        }
    }
}