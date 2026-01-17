using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using practice.DTOs;
using practice.Models;
using practice.Repository.Interface;
using practice.Services;
using System.Security.Claims;

namespace practice.Controllers
{
    [Authorize(Roles = "Voter")]
    public class VoterController : Controller
    {
        private readonly IVoterService _voterService;

        private readonly IElectionRepository _electionRepo;


        public VoterController(IVoterService voterService, IElectionRepository electionRepo)
        {
            _voterService = voterService;
            _electionRepo = electionRepo;
        }

        private int GetUserId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");

        public async Task<IActionResult> Dashboard()
        {
            var data = await _voterService.GetDashboardAsync(GetUserId());

            if (data == null || !data.User.IsVerified)
            {
                TempData["ErrorMessage"] = "Account pending verification.";
                return RedirectToAction("Login", "Auth");
            }

            ViewBag.User = data.User;
            ViewBag.ActiveElection = data.ActiveElections;
            ViewBag.VotedElectionIds = data.VotedElectionIds;

            return View();
        }


        public async Task<IActionResult> Vote(int electionId)
        {
            var (election, candidates) = await _voterService.GetVotingPageAsync(GetUserId(), electionId);

            if (election == null)
            {
                TempData["ErrorMessage"] = "Voting not available (Closed or already voted).";
                return RedirectToAction(nameof(Dashboard));
            }

            ViewBag.Election = election;
            return View(candidates);
        }

        // POST: Submit Vote
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitVote(VoteDto voteDto)
        {


            var result = await _voterService.CastVoteAsync(GetUserId(), voteDto);

            if (result == "Success")
            {
                TempData["SuccessMessage"] = "Your vote has been recorded successfully!";
                return RedirectToAction(nameof(Dashboard));
            }
            else
            {
                TempData["ErrorMessage"] = result;
                return RedirectToAction(nameof(Dashboard));
            }
        }

        // GET: Results
        // GET: Results
        public async Task<IActionResult> Results(int? electionId)
        {
            // CASE 1: No ID provided (User clicked "Results" in Navbar) -> SHOW LIST
            if (electionId == null)
            {
                var allElections = await _electionRepo.GetAllElectionsAsync();

                // Only show Active or Published elections
                var list = allElections
                    .Where(e => e.IsActive || e.ResultsPublished)
                    .OrderByDescending(e => e.StartDate)
                    .ToList();

                // We render a DIFFERENT view for the list
                return View("ResultsList", list);
            }

            // CASE 2: ID provided (User clicked a button) -> SHOW TABLE (Your existing logic)
            var election = await _electionRepo.GetElectionByIdAsync(electionId.Value);

            if (election == null || (!election.ResultsPublished))
            {
                TempData["ErrorMessage"] = "Results not available.";
                return RedirectToAction(nameof(Results));
            }

            var results = await _electionRepo.GetVoteResultsAsync(election.Id);
            ViewBag.Election = election;
            ViewBag.TotalVotes = results.Sum(r => r.TotalVotes);

            return View(results); // This loads your existing Results.cshtml
        }
        


        // GET: Edit Profile Page
        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            var userId = GetUserId();

            // 1. Get current dashboard data
            var data = await _voterService.GetDashboardAsync(userId);

            if (data?.User == null) return NotFound();

            // 2. Map the User entity to the DTO
            // This ensures the view gets exactly the data it expects
            var profileDto = new UpdateProfileDto
            {
                Id = data.User.Id,
                FullName = data.User.FullName,
                PhoneNumber = data.User.PhoneNumber
            };

            return View(profileDto);
        }

        // [POST] Submit Profile Changes
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(UpdateProfileDto dto)
        {
            var userId = GetUserId();

            // 1. Security Check: Ensure user is editing their own ID
            if (userId != dto.Id)
            {
                return Unauthorized();
            }

            // 2. Validate the DTO (Name & Phone format)
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            // 3. Call the Service
            // The service now correctly accepts UpdateProfileDto
            bool result = await _voterService.UpdateProfileAsync(dto);

            if (result)
            {
                TempData["SuccessMessage"] = "Profile updated successfully!";
                return RedirectToAction(nameof(Dashboard));
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to update profile.";
                return View(dto);
            }
        }
    }
}