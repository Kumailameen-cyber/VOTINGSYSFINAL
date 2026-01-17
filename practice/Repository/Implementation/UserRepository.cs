using Microsoft.EntityFrameworkCore;
using practice.Data;
using practice.Models;
using practice.Repository.Interface;

namespace practice.Repository.Implementation
{
    public class UserRepository: IUserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> checkEmailPreExisting(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }
        public async Task<bool> checkNumberPreExisting(string number)
        {
            return await _context.Users.AnyAsync(u => u.PhoneNumber == number);
        }
        public async Task<bool> checkCnicPreExisting(string cnic)
        {
            return await _context.Users.AnyAsync(u => u.cnic == cnic);
        }
        public async Task<User?> FindUserWithEmailAndActive(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email && u.IsActive);
        }

        public async Task<bool> checkVoterIdPreExisting(string voterId)
        {
            return await _context.Users.AnyAsync(u => u.VoterIdNumber == voterId);
        }

        public async Task<bool> AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddUserAsyncTransaction(User user)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // 2. Add to tracking
                await _context.Users.AddAsync(user);

                // 3. Save to DB (this might fail if email is duplicate, etc.)
                await _context.SaveChangesAsync();

                // 4. Commit transaction (Permanent Save)
                await transaction.CommitAsync();

                return true;
            }
            catch (Exception)
            {
                // 5. Rollback if anything went wrong
                await transaction.RollbackAsync();

                // Return false so the Service knows it failed
                return false;
            }
        }

        public async Task<User?> FindUserWithEmail(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> FindUserViaId(int Id)
        {
            return await _context.Users.FindAsync(Id);
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            // This forces EF Core to see the object as "Modified"
            _context.Users.Update(user);

            // Save changes and return true if rows were affected
            return await _context.SaveChangesAsync() > 0;
        }


        //Admin Methods

        public async Task<int> GetTotalVotersCountAsync()
        {
            return await _context.Users.CountAsync(u => u.Role == "Voter"
            && u.IsActive == true);
        }

        public async Task<int> GetPendingVerificationsCountAsync()
        {
            return await _context.Users.CountAsync(u => !u.IsVerified);
        }

        public async Task<List<User>> GetRecentPendingUsersAsync()
        {
            return await _context.Users
                .Where(u => !u.IsVerified)
                .OrderByDescending(u => u.CreatedAt)
                .Take(5)
                .ToListAsync();
        }


        public async Task<List<User>> GetAllUsersExcludingAdminAsync()
        {
            return await _context.Users
                .Where(u => u.Role != "Admin")
                .OrderByDescending(u => u.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<User>> GetAllUsersVotersAsync()
        {
            return await _context.Users
                .Where(u => u.Role != "Admin" && u.Role != "Candidate")
                .OrderByDescending(u => u.CreatedAt)
                .ToListAsync();
        }

        public async Task<bool> RemoveVoterAsync(int userId)
        {
            var Voter = await _context.Users.FindAsync(userId);

            // Always check for null!
            if (Voter == null) return false;
            var userVotes = _context.Votes.Where(v => v.VoterId == userId);
            if (userVotes.Any())
            {
                _context.Votes.RemoveRange(userVotes);
            }
            _context.Users.Remove(Voter);

            // SaveChanges returns the number of rows affected
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(User user)
        {
            try
            {
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
               
                return false;
            }
        }
    }
}
