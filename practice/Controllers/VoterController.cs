using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using practice.Data;
using practice.DTOs;
using practice.Models;
using System.Security.Claims;

namespace practice.Controllers
{
    [Authorize(Roles = "Voter")]
    public class VoterController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VoterController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Voter Dashboard
        public async Task<IActionResult> Dashboard()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
            var user = await _context.Users.FindAsync(userId);

            if (user == null || !user.IsVerified)
            {
                TempData["ErrorMessage"] = "Your account is pending verification. Please wait for admin approval.";
                return RedirectToAction("Login", "Auth");
            }

            var now = DateTime.Now;

            // Get active elections
            var activeElections = await _context.Elections
                .Where(e =>
                    e.IsActive &&
                    e.StartDate <= now &&
                    e.EndDate >= now
                )
                .OrderBy(e => e.StartDate)
                .ToListAsync();

            // Get IDs of elections the voter has already voted in
            var votedElectionIds = await _context.Votes
                .Where(v => v.VoterId == userId)
                .Select(v => v.ElectionId)
                .ToListAsync();

            ViewBag.User = user;
            ViewBag.ActiveElection = activeElections;
            ViewBag.VotedElectionIds = votedElectionIds;

            return View();
        }

        // GET: Vote Page
        public async Task<IActionResult> Vote(int electionId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
            var user = await _context.Users.FindAsync(userId);

            if (user == null || !user.IsVerified)
            {
                TempData["ErrorMessage"] = "Your account is not verified.";
                return RedirectToAction(nameof(Dashboard));
            }

            var election = await _context.Elections.FindAsync(electionId);
            if (election == null || !election.IsActive || DateTime.Now < election.StartDate || DateTime.Now > election.EndDate)
            {
                TempData["ErrorMessage"] = "Voting is not open for this election.";
                return RedirectToAction(nameof(Dashboard));
            }

            // Check if already voted
            var hasVoted = await _context.Votes
                .AnyAsync(v => v.ElectionId == electionId && v.VoterId == userId);
            if (hasVoted)
            {
                TempData["ErrorMessage"] = "You have already voted in this election.";
                return RedirectToAction(nameof(Dashboard));
            }

            // Get approved candidates
            var candidates = await _context.Candidates
                .Include(c => c.User)
                .Where(c => c.IsApproved)
                .Select(c => new CandidateDto
                {
                    Id = c.Id,
                    FullName = c.User.FullName,
                    PartyName = c.PartyName,
                    PartySymbol = c.PartySymbol,
                    Manifesto = c.Manifesto,
                    Biography = c.Biography,
                    Education = c.Education,
                    PreviousExperience = c.PreviousExperience,
                    ProfileImageUrl = c.ProfileImageUrl,
                    TotalVotes = c.TotalVotes
                })
                .ToListAsync();

            ViewBag.Election = election;
            return View(candidates);
        }

        // POST: Submit Vote
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitVote(VoteDto voteDto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");

            var election = await _context.Elections.FindAsync(voteDto.ElectionId);
            if (election == null || !election.IsActive || DateTime.Now < election.StartDate || DateTime.Now > election.EndDate)
            {
                TempData["ErrorMessage"] = "Invalid election or voting period.";
                return RedirectToAction(nameof(Dashboard));
            }

            var hasVoted = await _context.Votes
                .AnyAsync(v => v.ElectionId == voteDto.ElectionId && v.VoterId == userId);
            if (hasVoted)
            {
                TempData["ErrorMessage"] = "You have already voted.";
                return RedirectToAction(nameof(Dashboard));
            }

            var candidate = await _context.Candidates.FindAsync(voteDto.CandidateId);
            if (candidate == null || !candidate.IsApproved)
            {
                TempData["ErrorMessage"] = "Invalid candidate.";
                return RedirectToAction(nameof(Vote), new { electionId = voteDto.ElectionId });
            }

            // Record vote
            var vote = new Vote
            {
                ElectionId = voteDto.ElectionId,
                VoterId = userId,
                CandidateId = voteDto.CandidateId,
                VotedAt = DateTime.Now,
                IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                IsVerified = true
            };

            _context.Votes.Add(vote);
            candidate.TotalVotes++;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Your vote has been recorded successfully!";
            return RedirectToAction(nameof(Dashboard));
        }

        // GET: Results
        public async Task<IActionResult> Results(int? electionId)
        {
            Election? election;

            if (electionId.HasValue)
            {
                election = await _context.Elections
                    .FirstOrDefaultAsync(e => e.Id == electionId.Value && e.ResultsPublished);
            }
            else
            {
                election = await _context.Elections
                    .Where(e => e.ResultsPublished)
                    .OrderByDescending(e => e.EndDate)
                    .FirstOrDefaultAsync();
            }

            if (election == null)
            {
                TempData["ErrorMessage"] = "No results available yet.";
                return RedirectToAction(nameof(Dashboard));
            }

            var totalVotes = await _context.Votes
                .Where(v => v.ElectionId == election.Id)
                .CountAsync();

            var results = await _context.Candidates
                .Include(c => c.User)
                .Include(c => c.Votes)
                .Where(c => c.Votes.Any(v => v.ElectionId == election.Id))
                .Select(c => new VoteResultDto
                {
                    CandidateId = c.Id,
                    CandidateName = c.User.FullName,
                    PartyName = c.PartyName,
                    PartySymbol = c.PartySymbol,
                    TotalVotes = c.Votes.Count(v => v.ElectionId == election.Id),
                    VotePercentage = totalVotes > 0 ? (c.Votes.Count(v => v.ElectionId == election.Id) * 100.0 / totalVotes) : 0
                })
                .OrderByDescending(r => r.TotalVotes)
                .ToListAsync();

            ViewBag.Election = election;
            ViewBag.TotalVotes = totalVotes;

            return View(results);
        }
    }
}
