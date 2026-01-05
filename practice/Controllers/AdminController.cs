using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using practice.Models;
using practice.Services;

namespace practice.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;
        private readonly IAuthService _authService;

        // ? Constructor only injects Services
        public AdminController(IAdminService adminService, IAuthService authService)
        {
            _adminService = adminService;
            _authService = authService;
        }

        // ==========================================
        // 1. DASHBOARD & USERS
        // ==========================================

        // GET: Admin Dashboard
        public async Task<IActionResult> Dashboard()
        {
            var stats = await _adminService.GetDashboardStatsAsync();

            ViewBag.TotalVoters = stats.TotalVoters;
            ViewBag.TotalCandidates = stats.TotalCandidates;
            ViewBag.PendingVerifications = stats.PendingVerifications;
            ViewBag.PendingApprovals = stats.PendingApprovals;
            ViewBag.ActiveElections = stats.ActiveElections;
            ViewBag.TotalVotesCast = stats.TotalVotesCast;
            ViewBag.RecentUsers = stats.RecentUsers;
            ViewBag.RecentCandidates = stats.RecentCandidates;

            return View();
        }

        // GET: Manage Users
        public async Task<IActionResult> ManageVoters()
        {
            var users = await _adminService.GetAllVotersAsync();
            return View(users);
        }

        // POST: Verify User (Triggers Email via AuthService)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyUser(int userId)
        {
            var success = await _authService.VerifyUserAsync(userId);

            if (success) TempData["SuccessMessage"] = "User verified successfully. Email sent.";
            else TempData["ErrorMessage"] = "Verification failed.";

            return RedirectToAction(nameof(ManageVoters));
        }

        // POST: Deactivate User
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeactivateUser(int userId)
        {
            var success = await _authService.DeactivateUserAsync(userId);

            if (success) TempData["SuccessMessage"] = "User deactivated successfully.";
            else TempData["ErrorMessage"] = "Deactivation failed.";

            return RedirectToAction(nameof(ManageVoters));
        }

        // ==========================================
        // 2. CANDIDATES
        // ==========================================

        // GET: Manage Candidates
        public async Task<IActionResult> ManageCandidates()
        {
            var candidates = await _adminService.GetAllCandidatesAsync();
            return View(candidates);
        }

        // POST: Approve Candidate (Triggers Email via AuthService)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveCandidate(int candidateId)
        {
            var success = await _authService.ApproveCandidateAsync(candidateId);

            if (success) TempData["SuccessMessage"] = "Candidate approved. Email sent.";
            else TempData["ErrorMessage"] = "Approval failed.";

            return RedirectToAction(nameof(ManageCandidates));
        }

        // POST: Reject Candidate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectCandidate(int candidateId)
        {
            var success = await _authService.RejectCandidateAsync(candidateId);

            if (success) TempData["SuccessMessage"] = "Candidate rejected.";
            else TempData["ErrorMessage"] = "Rejection failed.";

            return RedirectToAction(nameof(ManageCandidates));
        }

        // ==========================================
        // 3. ELECTIONS
        // ==========================================

        // GET: Manage Elections
        public async Task<IActionResult> ManageElections()
        {
            var elections = await _adminService.GetAllElectionsAsync();
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
                var success = await _adminService.CreateElectionAsync(election);
                if (success)
                {
                    TempData["SuccessMessage"] = "Election created successfully!";
                    return RedirectToAction(nameof(ManageElections));
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to create Election";
                }
            }
            return View(election);
        }
        // ==========================================
        // EDIT ELECTION
        // ==========================================

        // GET: Edit Election
        public async Task<IActionResult> EditElection(int id)
        {
            var election = await _adminService.GetElectionByIdAsync(id);
            if (election == null)
            {
                TempData["ErrorMessage"] = "Election not found.";
                return RedirectToAction(nameof(ManageElections));
            }
            return View(election);
        }

        // POST: Edit Election
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditElection(Election election)
        {
            // Handle the disabled StartDate logic (same as Create)
            if (election.IsActive && election.StartDate == default)
            {
                // Retrieve original start date if needed, or keep existing logic
                // But usually, if editing an active election, we just ensure validation passes
                ModelState.Remove("StartDate");
            }

            if (ModelState.IsValid)
            {
                var success = await _adminService.UpdateElectionAsync(election);
                if (success)
                {
                    TempData["SuccessMessage"] = "Election updated successfully!";
                    return RedirectToAction(nameof(ManageElections));
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to update election.";
                }
            }
            return View(election);
        }

        // ==========================================
        // DELETE ELECTION
        // ==========================================



        // POST: Delete Election (Called directly via popup)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteElection(int id)
        {
            var success = await _adminService.DeleteElectionAsync(id);
            if (success)
            {
                TempData["SuccessMessage"] = "Election deleted successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to delete election.";
            }
            return RedirectToAction(nameof(ManageElections));
        }

        // POST: Publish Results
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PublishResults(int electionId)
        {
            var success = await _adminService.PublishElectionResultsAsync(electionId);

            if (success) TempData["SuccessMessage"] = "Results published successfully.";
            else TempData["ErrorMessage"] = "Failed to publish results.";

            return RedirectToAction(nameof(ManageElections));
        }

        // POST: Toggle Election Status (Activate/Deactivate)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleElectionStatus(int electionId)
        {
            var success = await _adminService.ToggleElectionStatusAsync(electionId);

            if (success) TempData["SuccessMessage"] = "Election status updated.";
            else TempData["ErrorMessage"] = "Failed to update status.";

            return RedirectToAction(nameof(ManageElections));
        }

        // GET: View Results
        public async Task<IActionResult> ViewResults(int electionId)
        {
            var election = await _adminService.GetElectionByIdAsync(electionId);
            if (election == null)
            {
                TempData["ErrorMessage"] = "Election not found.";
                return RedirectToAction(nameof(ManageElections));
            }

            // Calls AdminService -> calls ElectionRepo -> calculates percentages
            var results = await _adminService.GetElectionResultsAsync(electionId);

            ViewBag.Election = election;
            ViewBag.TotalVotes = results.Sum(r => r.TotalVotes);



            return View(results);
        }


    }
}