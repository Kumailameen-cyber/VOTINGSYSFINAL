using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using practice.DTOs;
using practice.Models;
using practice.Services;
using System.Security.Claims;

namespace practice.Controllers
{
    [Authorize(Roles = "Candidate")]
    public class CandidateController : Controller
    {
        private readonly ICandidateService _candidateService;

        // ? Inject Service Only (No Database Context)
        public CandidateController(ICandidateService candidateService)
        {
            _candidateService = candidateService;
        }

        // Helper to get logged-in User ID
        private int GetUserId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");

        // GET: Candidate Dashboard
        public async Task<IActionResult> Dashboard()
        {
            var data = await _candidateService.GetDashboardAsync(GetUserId());
            

            if (data == null || data.Candidate==null)
            {
                TempData["ErrorMessage"] = "Candidate profile not found.";
                return RedirectToAction("Login", "Auth");
            }

            if (!data.User.IsVerified || !data.Candidate.IsApproved)
            {
                TempData["WarningMessage"] = "Your profile is pending admin approval. You will be notified once approved.";
            }

            ViewBag.User = data.User;
            ViewBag.Candidate = data.Candidate;
            ViewBag.ActiveElection = data.ActiveElections;
            ViewBag.TotalVotes = data.Candidate.TotalVotes;
            ViewBag.VotesByElection = data.VotesByElection;

            return View();
        }

        // GET: Edit Profile
        public async Task<IActionResult> EditProfile()
        {
            var model = await _candidateService.GetProfileForEditAsync(GetUserId());
            if (model == null) return NotFound();

            return View(model);
        }

        // POST: Update Profile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(CandidateDto model)
        {
            var success = await _candidateService.UpdateProfileAsync(GetUserId(), model);

            if (!success) return NotFound();

            TempData["SuccessMessage"] = "Profile updated successfully!";
            return RedirectToAction(nameof(Dashboard));
        }

        // GET: View Results
        public async Task<IActionResult> Results(int? electionId)
        {
            // The service returns a Tuple: (Election, List of Results, My Vote Count)
            var (election, results, myVotes) = await _candidateService.GetResultsAsync(GetUserId(), electionId);

            if (election == null)
            {
                TempData["ErrorMessage"] = "No election found.";
                return RedirectToAction(nameof(Dashboard));
            }

            ViewBag.Election = election;
            ViewBag.TotalVotes = results.Sum(r => r.TotalVotes);
            ViewBag.MyVotes = myVotes;

            return View(results);
        }
        // POST: Participate in Election
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Participate(int electionId)
        {
            var userId = GetUserId();
            var candidate = await _candidateService.GetCandidateByUserIdServiceAsync(userId);
            bool participationResult = await _candidateService.ParticipateInElectionAsync(candidate, electionId);
           

            if(participationResult)
            {
                TempData["SuccessMessage"] = "Successfully registered for the election.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to register for the election. You may already be registered.";
            }
            return RedirectToAction(nameof(Dashboard) );
        }
    }
}