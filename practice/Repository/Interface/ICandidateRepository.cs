using practice.Models;

namespace practice.Repository.Interface
{
    public interface ICandidateRepository
    {
        Task<bool> RegisterCandidateAsync(User user, Candidate candidate);
    }
}
