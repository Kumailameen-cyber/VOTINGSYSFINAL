using practice.Models;

namespace practice.Repository.Interface
{
    public interface IVoterRepository
    {
        public Task<string> GetVoterNameByIdAsync(int voterId);
        public Task<string> GetVoterEmailByIdAsync(int voterId);

        
    }
}
