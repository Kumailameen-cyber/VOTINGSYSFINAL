using practice.DTOs;
using practice.Models;
using practice.Repository.Interface;

namespace practice.Services
{
    public class CandidateService : ICandidateService
    {
        private readonly ICandidateRepository _candidateRepo;
        private readonly IElectionRepository _electionRepo;

        public CandidateService(ICandidateRepository candidateRepo, IElectionRepository electionRepo)
        {
            _candidateRepo = candidateRepo;
            _electionRepo = electionRepo;
        }
        public async Task<int> GetCandidateByUserIdServiceAsync(int id)
        {
            return await _candidateRepo.GetCandidateIdByUserIdAsync(id);
        }
        public async Task<CandidateDashboardDto?> GetDashboardAsync(int userId)
        {
            // 1. Get Candidate details
            var candidate = await _candidateRepo.GetCandidateByUserIdAsync(userId);
            if (candidate == null) return null;

            // 2. Get Ongoing Elections
            var activeElections = await _electionRepo.GetOngoingElectionsAsync();

            // 3. Calculate Vote Stats (Moved from Controller)
            var votesByElection = candidate.Votes
                .GroupBy(v => v.ElectionId)
                .Select(g => new { ElectionId = g.Key, VoteCount = g.Count() })
                .ToList<dynamic>();

            return new CandidateDashboardDto
            {
                User = candidate.User,
                Candidate = candidate,
                ActiveElections = activeElections,
                VotesByElection = votesByElection
            };
        }

        public async Task<CandidateDto?> GetProfileForEditAsync(int userId)
        {
            var candidate = await _candidateRepo.GetCandidateByUserIdAsync(userId);
            if (candidate == null) return null;

            // Map Entity to DTO
            return new CandidateDto
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
        }

        public async Task<bool> UpdateProfileAsync(int userId, CandidateDto dto)
        {
            var candidate = await _candidateRepo.GetCandidateByUserIdAsync(userId);
            if (candidate == null) return false;

            // Update fields
            candidate.PartyName = dto.PartyName;
            candidate.PartySymbol = dto.PartySymbol;
            candidate.Manifesto = dto.Manifesto;
            candidate.Biography = dto.Biography;
            candidate.Education = dto.Education;
            candidate.PreviousExperience = dto.PreviousExperience;
            candidate.ProfileImageUrl = dto.ProfileImageUrl;

            return await _candidateRepo.UpdateCandidateAsync(candidate);
        }

        public async Task<(Election? Election, List<VoteResultDto> Results, int MyVotes)> GetResultsAsync(int userId, int? electionId)
        {
            var candidate = await _candidateRepo.GetCandidateByUserIdAsync(userId);
            if (candidate == null) return (null, new List<VoteResultDto>(), 0);

            // 1. Determine which election to show
            Election? election;
            if (electionId.HasValue)
                election = await _electionRepo.GetElectionByIdAsync(electionId.Value);
            else
                election = await _electionRepo.GetLatestActiveOrPublishedElectionAsync();

            if (election == null) return (null, new List<VoteResultDto>(), 0);

            // 2. Get Results (Reusing the logic from ElectionRepository)
            var results = await _electionRepo.GetVoteResultsAsync(election.Id);

            // 3. Find "My Votes" count from the results list
            var myVotes = results.FirstOrDefault(r => r.CandidateId == candidate.Id)?.TotalVotes ?? 0;

            return (election, results, myVotes);
        }

        public async Task<bool> ParticipateInElectionAsync(int candidateId, int electionId)
        {
            var candidate = await _candidateRepo.GetCandidateByIdAsync(candidateId);
            if (candidate == null || !candidate.IsApproved) return false;

            
            if (candidate.ElectionId != null)
            {
                var currentElection = await _electionRepo.GetElectionByIdAsync(candidate.ElectionId.Value);

                
                if (currentElection != null && currentElection.EndDate > DateTime.UtcNow)
                {
                    return false;
                }
            }

           
            var newElection = await _electionRepo.GetElectionByIdAsync(electionId);
            if (newElection == null || !newElection.IsActive) return false;

           
            if (candidate.ElectionId == electionId) return false;

            
            return await _candidateRepo.AddCandidateToElectionAsync(candidateId, electionId);
        }


    }
}