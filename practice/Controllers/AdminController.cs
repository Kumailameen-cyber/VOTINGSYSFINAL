using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using practice.Data;
using practice.DTOs;
using practice.Models;

namespace practice.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin Dashboard
        public async Task<IActionResult> Dashboard()
        {
            var totalVoters = await _context.Users.CountAsync(u => u.Role == "Voter");
            var totalCandidates = await _context.Candidates.CountAsync();
            var pendingVerifications = await _context.Users.CountAsync(u => !u.IsVerified);
            var pendingApprovals = await _context.Candidates.CountAsync(c => !c.IsApproved);
            var activeElections = await _context.Elections.CountAsync(e => e.IsActive);
            var totalVotesCast = await _context.Votes.CountAsync();

            ViewBag.TotalVoters = totalVoters;
            ViewBag.TotalCandidates = totalCandidates;
            ViewBag.PendingVerifications = pendingVerifications;
            ViewBag.PendingApprovals = pendingApprovals;
            ViewBag.ActiveElections = activeElections;
            ViewBag.TotalVotesCast = totalVotesCast;

            // Get recent registrations
            var recentUsers = await _context.Users
                .Where(u => !u.IsVerified)
                .OrderByDescending(u => u.CreatedAt)
                .Take(5)
                .ToListAsync();

            var recentCandidates = await _context.Candidates
                .Include(c => c.User)
                .Where(c => !c.IsApproved)
                .OrderByDescending(c => c.RegisteredAt)
                .Take(5)
                .ToListAsync();

            ViewBag.RecentUsers = recentUsers;
            ViewBag.RecentCandidates = recentCandidates;

            return View();
        }

        // GET: Manage Users
        public async Task<IActionResult> ManageUsers()
        {
            var users = await _context.Users
                .Where(u => u.Role != "Admin")
                .OrderByDescending(u => u.CreatedAt)
                .ToListAsync();

            return View(users);
        }

        // GET: Manage Voters (ONLY voters)
        public async Task<IActionResult> ManageVoters()
        {
            var voters = await _context.Users
                .Where(u => u.Role == "Voter")
                .OrderByDescending(u => u.CreatedAt)
                .ToListAsync();

            return View(voters);
        }


        // POST: Verify User
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyUser(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                user.IsVerified = true;
                user.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"User {user.FullName} verified successfully.";
            }

            return RedirectToAction(nameof(ManageUsers));
        }

        // POST: Deactivate User
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeactivateUser(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                user.IsActive = false;
                user.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"User {user.FullName} deactivated successfully.";
            }

            return RedirectToAction(nameof(ManageUsers));
        }

        // GET: Manage Candidates
        public async Task<IActionResult> ManageCandidates()
        {
            var candidates = await _context.Candidates
                .Include(c => c.User)
                .OrderByDescending(c => c.RegisteredAt)
                .ToListAsync();

            return View(candidates);
        }

        // POST: Approve Candidate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveCandidate(int candidateId)
        {
            var candidate = await _context.Candidates
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.Id == candidateId);

            if (candidate != null)
            {
                candidate.IsApproved = true;
                candidate.User.IsVerified = true;
                candidate.User.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Candidate {candidate.User.FullName} approved successfully.";
            }

            return RedirectToAction(nameof(ManageCandidates));
        }

        // POST: Reject Candidate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectCandidate(int candidateId)
        {
            var candidate = await _context.Candidates
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.Id == candidateId);

            if (candidate != null)
            {
                candidate.User.IsActive = false;
                candidate.User.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Candidate {candidate.User.FullName} rejected.";
            }

            return RedirectToAction(nameof(ManageCandidates));
        }

        // GET: Manage Elections
        public async Task<IActionResult> ManageElections()
        {
            var elections = await _context.Elections
                .OrderByDescending(e => e.CreatedAt)
                .ToListAsync();

            return View(elections);
        }

        // GET: Create Election
        public IActionResult CreateElection()
        {
            return View();
        }

        // POST: Create Election
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateElection(Election election)
        {
            if (ModelState.IsValid)
            {
                election.CreatedAt = DateTime.UtcNow;
                _context.Elections.Add(election);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Election created successfully!";
                return RedirectToAction(nameof(ManageElections));
            }

            return View(election);
        }

        // POST: Publish Results
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PublishResults(int electionId)
        {
            var election = await _context.Elections.FindAsync(electionId);
            if (election != null)
            {
                election.ResultsPublished = true;
                election.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Results for {election.Title} published successfully.";
            }

            return RedirectToAction(nameof(ManageElections));
        }

        // POST: Toggle Election Status
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleElectionStatus(int electionId)
        {
            var election = await _context.Elections.FindAsync(electionId);
            if (election != null)
            {
                election.IsActive = !election.IsActive;
                election.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                var status = election.IsActive ? "activated" : "deactivated";
                TempData["SuccessMessage"] = $"Election {election.Title} {status} successfully.";
            }

            return RedirectToAction(nameof(ManageElections));
        }

        // GET: View Results
        public async Task<IActionResult> ViewResults(int electionId)
        {
            var election = await _context.Elections.FindAsync(electionId);
            if (election == null)
            {
                TempData["ErrorMessage"] = "Election not found.";
                return RedirectToAction(nameof(ManageElections));
            }

            var totalVotes = await _context.Votes
                .Where(v => v.ElectionId == electionId)
                .CountAsync();

            var results = await _context.Candidates
                .Include(c => c.User)
                .Include(c => c.Votes)
                .Where(c => c.Votes.Any(v => v.ElectionId == electionId))
                .Select(c => new VoteResultDto
                {
                    CandidateId = c.Id,
                    CandidateName = c.User.FullName,
                    PartyName = c.PartyName,
                    PartySymbol = c.PartySymbol,
                    TotalVotes = c.Votes.Count(v => v.ElectionId == electionId),
                    VotePercentage = totalVotes > 0 ? (c.Votes.Count(v => v.ElectionId == electionId) * 100.0 / totalVotes) : 0
                })
                .OrderByDescending(r => r.TotalVotes)
                .ToListAsync();

            ViewBag.Election = election;
            ViewBag.TotalVotes = totalVotes;

            return View(results);
        }
    }
}
