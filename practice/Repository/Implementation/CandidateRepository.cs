using Microsoft.EntityFrameworkCore;
using practice.Data;
using practice.Models;
using practice.Repository.Interface;

namespace practice.Repository.Implementation
{
    public class CandidateRepository : ICandidateRepository
    {
        private readonly ApplicationDbContext _context;
        public CandidateRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> RegisterCandidateAsync(User user, Candidate candidate)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // 1. Save the User first (to generate the Id)
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();

                // 2. IMPORTANT: Link the new User Id to the Candidate
                candidate.UserId = user.Id;

                // 3. Save the Candidate
                await _context.Candidates.AddAsync(candidate);
                await _context.SaveChangesAsync();

                // 4. Commit
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception)
            {
                // 5. Rollback on any error
                await transaction.RollbackAsync();
                return false;
            }
        }

        //Admin Methods

        // Implement the methods similar to UserRepository, using _context.Candidates
        public async Task<int> GetTotalCandidatesCountAsync() => await _context.Candidates.CountAsync(
            u => u.User.Role == "Candidate"
            && u.User.IsActive == true);

        public async Task<int> GetPendingApprovalsCountAsync() => await _context.Candidates.CountAsync(c => !c.IsApproved);

        public async Task<List<Candidate>> GetRecentPendingCandidatesAsync()
        {
            return await _context.Candidates.Include(c => c.User)
                .Where(c => !c.IsApproved).OrderByDescending(c => c.RegisteredAt).Take(5).ToListAsync();
        }

        public async Task<List<Candidate>> GetAllCandidatesWithUsersAsync()
        {
            return await _context.Candidates.Include(c => c.User).OrderByDescending(c => c.RegisteredAt).ToListAsync();
        }

        public async Task<Candidate?> GetCandidateByIdAsync(int id)
        {
            return await _context.Candidates.Include(c => c.User).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<bool> UpdateCandidateAsync(Candidate candidate)
        {
            _context.Candidates.Update(candidate);
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<Candidate?> GetCandidateByUserIdAsync(int userId)
        {
            return await _context.Candidates
                .Include(c => c.User)
                .Include(c => c.Votes) // Important: Dashboard needs to count votes
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }
        public async Task<List<Candidate>> GetApprovedCandidatesAsync()
        {
            return await _context.Candidates
                .Include(c => c.User)
                .Where(c => c.IsApproved)
                .ToListAsync();
        }
        public async Task<bool> IsCandidateInElectionAsync(int candidateId, int electionId)
        {
            // Check if a candidate exists who matches BOTH the candidateId AND the electionId
            var isValid = await _context.Candidates
                .AnyAsync(c => c.Id == candidateId && c.ElectionId == electionId && c.IsApproved);

            return isValid;
        }
        public async Task<bool> AddCandidateToElectionAsync(int candidateId, int electionId)
        {
            // 1. Find the existing candidate
            var candidate = await _context.Candidates.FindAsync(candidateId);

            // 2. Safety check: does the candidate exist?
            if (candidate == null) return false;

            // 3. Update the link
            candidate.ElectionId = electionId;

            // 4. Save the change (returns true if database was updated)
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> AddCandidateAsync(Candidate candidate)
        {
            _context.Candidates.Add(candidate);
            return await _context.SaveChangesAsync() > 0;
        }


        public async Task<int> GetCandidateIdByUserIdAsync(int id)
        {
            return await _context.Candidates
                 .Where(c => c.UserId == id)
                 .Select(c => c.Id)
                 .FirstOrDefaultAsync();
        }

        public async Task<List<Candidate>> AllCandidateInElectionAsync(int electionId)
        {
            return await _context.Candidates
                .Include(c => c.User)
                .Where(c => c.ElectionId == electionId && c.IsApproved)
                .ToListAsync();
        }

        public async Task<bool> RemoveCandidateAsync(int candidateId)
        {
            var candidate = await _context.Candidates.FindAsync(candidateId);
            if (candidate == null) return false;

            // --- NEW CODE START ---
            // Just like you did for Voters, we must find votes FOR this candidate
            var candidateVotes = _context.Votes.Where(v => v.CandidateId == candidateId);

            // And delete them first
            if (candidateVotes.Any())
            {
                _context.Votes.RemoveRange(candidateVotes);
            }
            // --- NEW CODE END ---

            _context.Candidates.Remove(candidate);

            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<int> RemoveCandidatefromElectionAsync(int CandidateId)
        {
            var candidate = await _context.Candidates.FindAsync(CandidateId);
            // Always check for null!
            if (candidate == null) return 0;
            int? newid = candidate.ElectionId;
            candidate.ElectionId = null;
            await _context.SaveChangesAsync();
            // SaveChanges returns the number of rows affected
            return (newid==null)?0:(int)newid;
        }

        public async Task<bool> ChangeStatusaftertime(int candidate)
        {
            var can = await _context.Candidates.FindAsync(candidate);
            if (can == null) return false;
            if(can.ElectionId.HasValue)
            {
                var election = await _context.Elections.FindAsync(can.ElectionId);
                if(election.EndDate<DateTime.Now)
                {
                    can.ElectionId = null;
                    await _context.SaveChangesAsync();
                    election.IsActive = false;
                    return true;
                }
            }

            return false;
        }
    }
}