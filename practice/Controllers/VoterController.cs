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
        public async Task<IActionResult> Results(int? electionId)
        {
            
            Election? election;
            if (electionId.HasValue)
                election = await _electionRepo.GetElectionByIdAsync(electionId.Value);
            else
                election = await _electionRepo.GetLatestActiveOrPublishedElectionAsync();

            if (election == null || (!election.ResultsPublished && !election.IsActive)) // Check rules for showing results
            {
                TempData["ErrorMessage"] = "No results available.";
                return RedirectToAction(nameof(Dashboard));
            }

            var results = await _electionRepo.GetVoteResultsAsync(election.Id);
            var totalVotes = results.Sum(r => r.TotalVotes);

            ViewBag.Election = election;
            ViewBag.TotalVotes = totalVotes;

            return View(results);
        }
    }
}