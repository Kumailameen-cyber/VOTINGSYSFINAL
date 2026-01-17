using Microsoft.AspNetCore.Mvc;
using practice.Models;
using practice.Repository.Interface;

namespace practice.Controllers
{
    public class PublicResultController : Controller
    {
        private readonly IElectionRepository _electionRepo;

        public PublicResultController(IElectionRepository electionRepo)
        {
            _electionRepo = electionRepo;
        }

        // GET: /PublicElection/Index
        // Shows the list of all published elections
        public async Task<IActionResult> Index()
        {
            var allElections = await _electionRepo.GetAllElectionsAsync();

            // Filter: Only Published elections
            var publishedElections = allElections
                .Where(e => e.ResultsPublished)
                .OrderByDescending(e => e.EndDate)
                .ToList();

            return View(publishedElections);
        }

        // GET: /PublicElection/Details/5
        //Shows the results for a specific election
        // GET: /PublicElection/Details/5
        public async Task<IActionResult> Details(int id)
        {
            // 1. Get the Election Data
            var election = await _electionRepo.GetElectionByIdAsync(id);

            // Security: If not found or not published, kick them back to list
            if (election == null || !election.ResultsPublished)
            {
                return RedirectToAction(nameof(Index));
            }

            // 2. Get the Results Data (Reusing your existing Repo method)
            // This returns IEnumerable<VoteResultDto> which your view expects
            var results = await _electionRepo.GetVoteResultsAsync(id);

            // 3. SET VIEW BAGS EXACTLY AS YOUR VIEW EXPECTS
            ViewBag.Election = election;
            ViewBag.TotalVotes = results.Sum(r => r.TotalVotes);

            return View(results);
        }
    }
}