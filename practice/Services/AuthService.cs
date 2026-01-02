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

        public AuthService(ITokenService tokenService, IUserRepository userRepository,
            ICandidateRepository candidateRepository)
        {
            _tokenService = tokenService;
            _repo_user = userRepository;
            _repo_candidate = candidateRepository;
        }

        public async Task<LoginResponseDto?> LoginAsync(LoginDto loginDto)
        {
            var user = await _repo_user.FindUserWithEmailAndActive (loginDto.Email);

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
                AadharNumber = registerDto.AadharNumber,
                VoterIdNumber = registerDto.VoterIdNumber,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
                Role = "Voter",
                IsVerified = false, // Will be verified by admin
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

             

            return await _repo_user.AddUserAsync(user);
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
                    AadharNumber = registerDto.AadharNumber,
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
                    RegisteredAt = DateTime.UtcNow
                };


            return await _repo_candidate.RegisterCandidateAsync(user, candidate);
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

            return await _repo_user.UpdateUserAsync(user);
        }
    }
}
