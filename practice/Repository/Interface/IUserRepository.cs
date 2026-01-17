using practice.DTOs;
using practice.Models;

namespace practice.Repository.Interface
{
    public interface IUserRepository
    {

        public Task<User?> FindUserWithEmailAndActive(string email);

        public Task<User?> FindUserWithEmail(string email);

        public Task<bool> checkEmailPreExisting(string email);
        public Task<bool> checkCnicPreExisting(string cnic);
        public Task<bool> checkNumberPreExisting(string number);

        public Task<bool> checkVoterIdPreExisting(string voterId);

        public Task<bool> AddUserAsync(User user);

        public Task<bool> AddUserAsyncTransaction(User user);

        public Task<User?> FindUserViaId(int Id);

        public Task<bool> UpdateUserAsync(User user);


        //Admin Methods

        public Task<int> GetTotalVotersCountAsync();
        public Task<int> GetPendingVerificationsCountAsync();
        public Task<List<User>> GetRecentPendingUsersAsync();
        public Task<List<User>> GetAllUsersExcludingAdminAsync();
        public Task<List<User>> GetAllUsersVotersAsync();
        public Task<bool> RemoveVoterAsync(int userId);

        Task<bool> UpdateAsync(User user);


    }
}
