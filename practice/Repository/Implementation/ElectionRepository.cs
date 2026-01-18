using Microsoft.EntityFrameworkCore;

using practice.Data;
using practice.DTOs;
using practice.Models;
using practice.Repository.Interface;

namespace practice.Repository.Implementation
{
    public class ElectionRepository : IElectionRepository
    {
        private readonly ApplicationDbContext _context;

        public ElectionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> ActiveElectionAsync()
        {
            return await _context.Elections.CountAsync(e => e.IsActive);
        }

        public async Task<int> TotalVotesCast()
        {
            // Logic: Count all votes where the related Election is Active.
            return await _context.Votes
                .Include(v => v.Election) // Load the Election info so we can check 'IsActive'
                .Where(v => v.Election.IsActive) // The Filter
                .CountAsync();
        }

        // --- NEW METHODS ---

        public async Task<List<Election>> GetAllElectionsAsync()
        {
            return await _context.Elections
                .OrderByDescending(e => e.CreatedAt)
                .ToListAsync();
        }

        public async Task<Election?> GetElectionByIdAsync(int id)
        {
            return await _context.Elections.FindAsync(id);
        }

        public async Task<bool> AddElectionAsync(Election election)
        {
            await _context.Elections.AddAsync(election);
            return await _context.SaveChangesAsync() > 0;
        }
        
        public async Task<bool> UpdateElectionAsync(Election election)
        {
            _context.Elections.Update(election);
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<bool> DeleteElectionAsync(int id)
        {
            // 1. Unlink (or Delete) VOTES first
            // Since the error says "FK_Votes_Elections_ElectionId", your Votes table has a direct link.
            var votes = await _context.Votes
                .Where(v => v.ElectionId == id) // Ensure your Vote model has this property
                .ToListAsync();

            if (votes.Any())
            {
                

                // Option B: If you want to keep history, set ElectionId to null (Requires int?)
                 foreach (var v in votes) { v.ElectionId = null; }
            }

            // 2. Unlink CANDIDATES (Your existing logic)
            var candidates = await _context.Candidates
                .Where(c => c.ElectionId == id)
                .ToListAsync();

            foreach (var candidate in candidates)
            {
                candidate.ElectionId = null;
            }

            // 3. Now delete the ELECTION
            var election = await _context.Elections.FindAsync(id);
            if (election == null) return false;

            _context.Elections.Remove(election);

            // 4. Save everything
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<List<VoteResultDto>> GetVoteResultsAsync(int electionId)
        {
            // 1. Get total votes for percentage calculation
            var totalVotes = await _context.Votes
                .Where(v => v.ElectionId == electionId)
                .CountAsync();

            // 2. Query Candidates based on ElectionId, NOT on if they have votes
            return await _context.Candidates
                .Include(c => c.User)
                .Include(c => c.Votes)
                .Where(c => c.ElectionId == electionId && c.IsApproved) // <--- CHANGED THIS LINE
                .Select(c => new VoteResultDto
                {
                    CandidateId = c.Id,
                    CandidateName = c.User.FullName,
                    PartyName = c.PartyName,
                    PartySymbol = c.PartySymbol,
                    PhotoUrl = c.ProfileImageUrl ?? "",

                    // Count specific votes for this election
                    TotalVotes = c.Votes.Count(v => v.ElectionId == electionId),

                    // Safe percentage calculation
                    VotePercentage = totalVotes > 0
                        ? (c.Votes.Count(v => v.ElectionId == electionId) * 100.0 / totalVotes)
                        : 0
                })
                .OrderByDescending(r => r.TotalVotes)
                .ToListAsync();
        }
        public async Task<List<Election>> GetOngoingElectionsAsync()
        {
            var now = DateTime.Now;
            return await _context.Elections
                .Where(e => e.IsActive && e.StartDate <= now && e.EndDate >= now)
                .OrderBy(e => e.StartDate)
                .ToListAsync();
        }

        public async Task<Election?> GetLatestActiveOrPublishedElectionAsync()
        {
            return await _context.Elections
                .Where(e => e.IsActive || e.ResultsPublished)
                .OrderByDescending(e => e.EndDate)
                .FirstOrDefaultAsync();
        }



        public async Task<List<int>> GetVotedElectionIdsAsync(int userId)
        {
            return await _context.Votes
                .Where(v => v.VoterId == userId && v.ElectionId != null)
                .Select(v => v.ElectionId.Value)
                .ToListAsync();
        }

        public async Task<bool> HasUserVotedAsync(int electionId, int userId)
        {
            return await _context.Votes
                .AnyAsync(v => v.ElectionId == electionId && v.VoterId == userId);
        }

        public async Task<bool> CastVoteAsync(Vote vote, Candidate candidate)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // 1. Add the Vote record
                await _context.Votes.AddAsync(vote);

                // 2. Increment the Candidate's total count
                candidate.TotalVotes++;
                _context.Candidates.Update(candidate);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                return false;
            }
        }

        public async Task<bool> ElectionTimeCheckerAsync()
        {
            // 1. Get ALL elections that are Active AND have passed their EndDate
            var expiredElections = await _context.Elections
                .Where(e => e.IsActive && e.EndDate < DateTime.Now)
                .ToListAsync(); // Fetch them all into memory

            // 2. Check if there is anything to update
            if (!expiredElections.Any())
            {
                return false; // Nothing to do
            }

            // 3. Loop through the list we just fetched
            foreach (var election in expiredElections)
            {
                election.IsActive = false;
                // No need to call .Update() explicitly if tracking is on, 
                // but keeping it is fine.
            }

            // 4. Save ALL changes in one go (Much faster than saving inside the loop)
            await _context.SaveChangesAsync();

            return true;
        }
    }
}