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

            // Check if voter ID already exists
            if (await _repo_user.checkVoterIdPreExisting(registerDto.VoterIdNumber))
                return false;

            var user = new User
            {
                FullName = registerDto.FullName,
                Email = registerDto.Email,
                PhoneNumber = registerDto.PhoneNumber,
                cnic = registerDto.cnic,
                VoterIdNumber = registerDto.VoterIdNumber,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
                Role = "Voter",
                IsVerified = false, // Will be verified by admin
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };



            var isSaved = await _repo_user.AddUserAsync(user);

            if (isSaved)
            {
                var subject = "Registration Successful - Verification Pending";
                var body = $"Hello {user.FullName},\n\n" +
                           "Thank you for registering on the Voting Portal.\n" +
                           "Your account has been created successfully.\n\n" +
                           "Please note: You cannot login yet. Your account is under review and will be verified within 2 business days.\n" +
                           "You will receive an email once an Admin verifies your details.";

                try
                {
                    await _emailService.SendEmailAsync(user.Email, subject, body);
                }
                catch
                {
                    // Email failed, but user is registered. We don't want to crash here.
                }
            }

            return isSaved;
        }

        public async Task<bool> RegisterCandidateAsync(RegisterCandidateDto registerDto)
        {
            // Check if email already exists
            if (await _repo_user.checkEmailPreExisting(registerDto.Email))
                return false;

            // Check if voter ID already exists
            if (await _repo_user.checkVoterIdPreExisting(registerDto.VoterIdNumber))
                return false;


            var user = new User
            {
                FullName = registerDto.FullName,
                Email = registerDto.Email,
                PhoneNumber = registerDto.PhoneNumber,
                cnic = registerDto.cnic,
                VoterIdNumber = registerDto.VoterIdNumber,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
                Role = "Candidate",
                IsVerified = false, // Will be verified by admin
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };



            var candidate = new Candidate
            {
                UserId = user.Id,
                PartyName = registerDto.PartyName,
                PartySymbol = registerDto.PartySymbol,
                Manifesto = registerDto.Manifesto,
                Biography = registerDto.Biography,
                Education = registerDto.Education,
                PreviousExperience = registerDto.PreviousExperience,
                IsApproved = false, // Will be approved by admin
                RegisteredAt = DateTime.UtcNow,
                Election = null
            };


            var isSaved = await _repo_candidate.RegisterCandidateAsync(user, candidate);
            if (isSaved)
            {
                var subject = "Candidate Application Received";
                var body = $"Hello {user.FullName},\n\n" +
                           "Your application to register as a Candidate has been received.\n" +
                           "The Election Commission (Admin) will review your Manifesto and details.\n\n" +
                           "This process may take up to 2 business days. You will be notified via email upon approval.";

                try
                {
                    await _emailService.SendEmailAsync(user.Email, subject, body);
                }
                catch
                {
                    // Ignore email errors
                }
            }
            return isSaved;
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
            candidate.User.IsActive = false;

            var isSaved = await _repo_candidate.UpdateCandidateAsync(candidate);

            if (isSaved)
            {
                // Optional: Send Rejection Email
                try
                {
                    await _emailService.SendEmailAsync(candidate.User.Email, "Application Update", "Your candidacy was not approved.");
                }
                catch { }
            }
            return isSaved;
        }

        public async Task<bool> DeactivateUserAsync(int userId)
        {
            var user = await _repo_user.FindUserViaId(userId);
            if (user == null) return false;

            user.IsActive = false;
            user.UpdatedAt = DateTime.UtcNow;

            return await _repo_user.UpdateUserAsync(user);
        }
    }
}

