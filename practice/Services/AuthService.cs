using Microsoft.EntityFrameworkCore;
using practice.Data;
using practice.DTOs;
using practice.Models;
using practice.Repository.Interface;

namespace practice.Services
{
    public class AuthService : IAuthService
    {

        private readonly ITokenService _tokenService;
        private readonly ICandidateRepository _repo_candidate;
        private readonly IUserRepository _repo_user;
        private readonly IEmailService _emailService;

        public AuthService(ITokenService tokenService, IUserRepository userRepository,
            ICandidateRepository candidateRepository, IEmailService emailService)
        {
            _tokenService = tokenService;
            _repo_user = userRepository;
            _repo_candidate = candidateRepository;
            _emailService = emailService;
        }

        public async Task<LoginResponseDto?> LoginAsync(LoginDto loginDto)
        {
            var user = await _repo_user.FindUserWithEmailAndActive(loginDto.Email);

            if (user == null)
                return null;

            if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
                return null;

            var token = _tokenService.GenerateToken(user);

            return new LoginResponseDto
            {
                Token = token,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role,
                UserId = user.Id,
                IsVerified = user.IsVerified
            };
        }

        public async Task<bool> RegisterVoterAsync(RegisterVoterDto registerDto)
        {
            // Check if email already exists
            if (await _repo_user.checkEmailPreExisting(registerDto.Email))
                return false;

            // also prevent duplicate CNIC
            // If you don't have this method, comment this out or implement it in repository.
            // if (await _repo_user.checkCnicPreExisting(registerDto.cnic))
            //     return false;

            var user = new User
            {
                FullName = registerDto.FullName,
                Email = registerDto.Email,
                PhoneNumber = registerDto.PhoneNumber,
                cnic = registerDto.cnic,
                VoterIdNumber = "", // temp, will be generated after saving
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
                Role = "Voter",
                IsVerified = false,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            // 1) Save user first so SQL generates user.Id
            var isSaved = await _repo_user.AddUserAsync(user);
            if (!isSaved) return false;

            // 2) Generate voter id using generated Id
            user.VoterIdNumber = $"VOT-{user.Id:D6}";

            // 3) Update user with voter id
            var isUpdated = await _repo_user.UpdateUserAsync(user);
            if (!isUpdated) return false;

            // Email (optional)
            var subject = "Registration Successful - Verification Pending";
            var body = $"Hello {user.FullName},\n\n" +
                       "Thank you for registering on the Voting Portal.\n" +
                       "Your account has been created successfully.\n\n" +
                       "Please note: You cannot login yet. Your account is under review and will be verified within 2 business days.\n" +
                       "You will receive an email once an Admin verifies your details.\n\n" +
                       $"Your Voter ID is: {user.VoterIdNumber}";

            try
            {
                await _emailService.SendEmailAsync(user.Email, subject, body);
            }
            catch
            {
                // ignore email errors
            }

            return true;
        }



        public async Task<bool> RegisterCandidateAsync(RegisterCandidateDto registerDto)
        {
            // Check if email already exists
            if (await _repo_user.checkEmailPreExisting(registerDto.Email))
                return false;

            // OPTIONAL (recommended): also prevent duplicate CNIC
            // if (await _repo_user.checkCnicPreExisting(registerDto.cnic))
            //     return false;

            var user = new User
            {
                FullName = registerDto.FullName,
                Email = registerDto.Email,
                PhoneNumber = registerDto.PhoneNumber,
                cnic = registerDto.cnic,
                VoterIdNumber = "", // temp, will be generated after saving
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
                Role = "Candidate",
                IsVerified = false,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            // 1) Save user first so SQL generates user.Id
            var userSaved = await _repo_user.AddUserAsync(user);
            if (!userSaved) return false;

            // 2) Generate voter id using generated Id
            user.VoterIdNumber = $"VOT-{user.Id:D6}";

            // 3) Update user with voter id
            var userUpdated = await _repo_user.UpdateUserAsync(user);
            if (!userUpdated) return false;

            // 4) Now create candidate using the real user.Id
            var candidate = new Candidate
            {
                UserId = user.Id,
                PartyName = registerDto.PartyName,
                PartySymbol = registerDto.PartySymbol,
                Manifesto = registerDto.Manifesto,
                Biography = registerDto.Biography,
                Education = registerDto.Education,
                PreviousExperience = registerDto.PreviousExperience,
                IsApproved = false,
                RegisteredAt = DateTime.UtcNow
            };

            // IMPORTANT:
            // You must have a repository method that saves candidate only, e.g. AddCandidateAsync(candidate)
            // If you do not have it, add it in ICandidateRepository and implement it.
            var candidateSaved = await _repo_candidate.AddCandidateAsync(candidate);
            if (!candidateSaved) return false;

            // Email (optional)
            var subject = "Candidate Application Received";
            var body = $"Hello {user.FullName},\n\n" +
                       "Your application to register as a Candidate has been received.\n" +
                       "The Election Commission (Admin) will review your Manifesto and details.\n\n" +
                       "This process may take up to 2 business days. You will be notified via email upon approval.\n\n" +
                       $"Your Voter ID is: {user.VoterIdNumber}";

            try
            {
                await _emailService.SendEmailAsync(user.Email, subject, body);
            }
            catch
            {
                // ignore email errors
            }

            return true;
        }


        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _repo_user.FindUserWithEmail(email);
        }

        public async Task<bool> VerifyUserAsync(int userId)
        {
            var user = await _repo_user.FindUserViaId(userId);
            if (user == null)
                return false;

            user.IsVerified = true;
            user.UpdatedAt = DateTime.UtcNow;

            var IsUpdated = await _repo_user.UpdateUserAsync(user);

            if (IsUpdated)
            {
                if (user.Role == "Voter")
                {
                    var subject = "Account Verified - You Can Now Vote!";
                    var body = $"Hello {user.FullName},\n\n" +
                               "Good news! Your account documents have been verified by the Admin.\n\n" +
                               "You can now login to the Voting Portal and cast your vote in the upcoming election.\n" +
                               "Login here: http://localhost:3000/login"; // Put your frontend link here if you have one

                    try
                    {
                        await _emailService.SendEmailAsync(user.Email, subject, body);
                    }
                    catch { }
                }
                else if (user.Role == "Candidate")
                {
                    var subject = "Candidate Account Verified - Awaiting Admin Approval";
                    var body = $"Hello {user.FullName},\n\n" +
                               "Your candidate account documents have been verified by the Admin.\n\n" +
                               "However, your candidacy is still pending final approval from the Admin team.\n" +
                               "You will receive another email once your candidacy is approved.";
                    try
                    {
                        await _emailService.SendEmailAsync(user.Email, subject, body);
                    }
                    catch { }
                }
                
            }
            return IsUpdated;
        }


        public async Task<bool> ApproveCandidateAsync(int candidateId)
        {
            // 1. Get Candidate
            var candidate = await _repo_candidate.GetCandidateByIdAsync(candidateId);
            if (candidate == null) return false;

            // 2. Update Logic
            candidate.IsApproved = true;
            candidate.User.IsVerified = true; // Auto-verify the user account too
            candidate.User.UpdatedAt = DateTime.UtcNow;

            // 3. Save Changes
            var isSaved = await _repo_candidate.UpdateCandidateAsync(candidate);

            // 4. Send Email
            if (isSaved)
            {
                var subject = "Candidacy Approved!";
                var body = $"Hello {candidate.User.FullName},\n\n" +
                           "Congratulations! Your application for candidacy has been APPROVED by the Admin.\n" +
                           "You can now campaign for the upcoming election.";
                try
                {
                    await _emailService.SendEmailAsync(candidate.User.Email, subject, body);
                }
                catch { }
            }
            return isSaved;
        }

        public async Task<bool> RejectCandidateAsync(int candidateId)
        {
            var candidate = await _repo_candidate.GetCandidateByIdAsync(candidateId);
            if (candidate == null) return false;

            // Logic: Maybe deactivate the user or just leave IsApproved = false
            
            await _emailService.SendEmailAsync(candidate.User.Email, "Application Update", "Your candidacy was not approved.");
               
            var isSaved = await _repo_candidate.RemoveCandidateAsync(candidateId);
            return isSaved;
        }

        public async Task<bool> DeactivateUserAsync(int userId)
        {
            var user = await _repo_user.FindUserViaId(userId);
            await _emailService.SendEmailAsync(user.Email, "Application Update", "Your candidacy was not approved.");



            var isSaved = await _repo_user.RemoveVoterAsync(userId);
            return isSaved;
            
        }
    }
}

