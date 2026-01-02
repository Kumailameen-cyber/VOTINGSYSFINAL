using practice.DTOs;
using practice.Models;

namespace practice.Repository.Interface
{
    public interface IUserRepository
    {

        public Task<User?> FindUserWithEmailAndActive(string email);

        public Task<User?> FindUserWithEmail(string email);

        public Task<bool> checkEmailPreExisting(string email);

        public Task<bool> checkVoterIdPreExisting(string voterId);

        public Task<bool> AddUserAsync(User user);

        public Task<bool> AddUserAsyncTransaction(User user);

        public Task<User?> FindUserViaId(int Id);

        public Task<bool> UpdateUserAsync(User user);




    }
}
