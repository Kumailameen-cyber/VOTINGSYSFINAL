using practice.Models;

namespace practice.Repository.Interface
{
    public interface ICandidateRepository
    {
        Task<bool> RegisterCandidateAsync(User user, Candidate candidate);

        //Admin Methods

        public Task<int> GetTotalCandidatesCountAsync();
        public Task<int> GetPendingApprovalsCountAsync();
        public Task<List<Candidate>> GetRecentPendingCandidatesAsync();
        public Task<List<Candidate>> GetAllCandidatesWithUsersAsync();
        public Task<Candidate?> GetCandidateByIdAsync(int id);
        public Task<bool> UpdateCandidateAsync(Candidate candidate);
        public Task<Candidate?> GetCandidateByUserIdAsync(int userId);
        public Task<List<Candidate>> GetApprovedCandidatesAsync();
        public Task<bool> IsCandidateInElectionAsync(int candidateId, int electionId);
        public Task<bool> AddCandidateToElectionAsync(int candidateId, int electionId);
        

        public Task<int> GetCandidateIdByUserIdAsync(int id);
        public Task<List<Candidate>> AllCandidateInElectionAsync(int electionId);

    }
}