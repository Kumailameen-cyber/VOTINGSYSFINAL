using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using practice.Data;
using practice.DTOs;
using practice.Models;
using System.Security.Claims;

namespace practice.Controllers
{
    [Authorize(Roles = "Candidate")]
    public class CandidateController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CandidateController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Candidate Dashboard
        public async Task<IActionResult> Dashboard()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");

            var user = await _context.Users.FindAsync(userId);
            var candidate = await _context.Candidates
                .Include(c => c.User)
                .Include(c => c.Votes)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (user == null || candidate == null)
            {
                TempData["ErrorMessage"] = "Candidate profile not found.";
                return RedirectToAction("Login", "Auth");
            }

            if (!user.IsVerified || !candidate.IsApproved)
            {
                TempData["WarningMessage"] = "Your profile is pending admin approval. You will be notified once approved.";
            }

            // Get active election
            var activeElection = await _context.Elections
                .Where(e => e.IsActive && e.StartDate <= DateTime.UtcNow && e.EndDate >= DateTime.UtcNow)
                .FirstOrDefaultAsync();

            // Get vote statistics for this candidate
            var votesByElection = await _context.Votes
                .Where(v => v.CandidateId == candidate.Id)
                .GroupBy(v => v.ElectionId)
                .Select(g => new
                {
                    ElectionId = g.Key,
                    VoteCount = g.Count()
                })
                .ToListAsync();

            ViewBag.User = user;
            ViewBag.Candidate = candidate;
            ViewBag.ActiveElection = activeElection;
            ViewBag.TotalVotes = candidate.TotalVotes;
            ViewBag.VotesByElection = votesByElection;

            return View();
        }

        // GET: Edit Profile
        public async Task<IActionResult> EditProfile()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
            var candidate = await _context.Candidates
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (candidate == null)
            {
                return NotFound();
            }

            var model = new CandidateDto
            {
                Id = candidate.Id,
                FullName = candidate.User.FullName,
                PartyName = candidate.PartyName,
                PartySymbol = candidate.PartySymbol,
                Manifesto = candidate.Manifesto,
                Biography = candidate.Biography,
                Education = candidate.Education,
                PreviousExperience = candidate.PreviousExperience,
                ProfileImageUrl = candidate.ProfileImageUrl
            };

            return View(model);
        }

        // POST: Update Profile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(CandidateDto model)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
            var candidate = await _context.Candidates
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (candidate == null)
            {
                return NotFound();
            }

            candidate.PartyName = model.PartyName;
            candidate.PartySymbol = model.PartySymbol;
            candidate.Manifesto = model.Manifesto;
            candidate.Biography = model.Biography;
            candidate.Education = model.Education;
            candidate.PreviousExperience = model.PreviousExperience;
            candidate.ProfileImageUrl = model.ProfileImageUrl;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Profile updated successfully!";
            return RedirectToAction(nameof(Dashboard));
        }

        // GET: View Results
        public async Task<IActionResult> Results(int? electionId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
            var candidate = await _context.Candidates
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (candidate == null)
            {
                return NotFound();
            }

            Models.Election? election;

            if (electionId.HasValue)
            {
                election = await _context.Elections
                    .FirstOrDefaultAsync(e => e.Id == electionId.Value);
            }
            else
            {
                election = await _context.Elections
                    .Where(e => e.IsActive || e.ResultsPublished)
                    .OrderByDescending(e => e.EndDate)
                    .FirstOrDefaultAsync();
            }

            if (election == null)
            {
                TempData["ErrorMessage"] = "No election found.";
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
            ViewBag.CandidateId = candidate.Id;
            ViewBag.MyVotes = results.FirstOrDefault(r => r.CandidateId == candidate.Id)?.TotalVotes ?? 0;

            return View(results);
        }
    }
}
