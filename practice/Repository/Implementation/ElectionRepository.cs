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
            return await _context.Votes.CountAsync();
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
        public async Task<bool> DeleteElectionAsync(int id)
        {
            var election = await _context.Elections.FindAsync(id);
            if (election == null) return false;

            _context.Elections.Remove(election);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateElectionAsync(Election election)
        {
            _context.Elections.Update(election);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<VoteResultDto>> GetVoteResultsAsync(int electionId)
        {
            // 1. Get total votes for percentage calculation
            var totalVotes = await _context.Votes
                .Where(v => v.ElectionId == electionId)
                .CountAsync();

            // 2. The complex LINQ query to group votes by candidate
            return await _context.Candidates
                .Include(c => c.User)
                .Include(c => c.Votes)
                .Where(c => c.Votes.Any(v => v.ElectionId == electionId)) // Only get candidates who got votes (optional)
                .Select(c => new VoteResultDto
                {
                    CandidateId = c.Id,
                    CandidateName = c.User.FullName,
                    PartyName = c.PartyName,
                    PartySymbol = c.PartySymbol,
                    TotalVotes = c.Votes.Count(v => v.ElectionId == electionId),

                    // Avoid divide by zero error
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
                .Where(v => v.VoterId == userId)
                .Select(v => v.ElectionId)
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
    }
}