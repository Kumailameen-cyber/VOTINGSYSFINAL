using practice.Models;

namespace practice.Repository.Interface
{
    public interface ICandidateRepository
    {
        Task<bool> RegisterCandidateAsync(User user, Candidate candidate);
        Task<bool> AddCandidateAsync(Candidate candidate);
        Task<Candidate?> GetCandidateByIdAsync(int id);
        Task<bool> UpdateCandidateAsync(Candidate candidate);
    }
}
