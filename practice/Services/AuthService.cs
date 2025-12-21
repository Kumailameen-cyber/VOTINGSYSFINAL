using Microsoft.EntityFrameworkCore;
using practice.Data;
using practice.DTOs;
using practice.Models;

namespace practice.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly ITokenService _tokenService;

        public AuthService(ApplicationDbContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<LoginResponseDto?> LoginAsync(LoginDto loginDto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == loginDto.Email && u.IsActive);

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
            if (await _context.Users.AnyAsync(u => u.Email == registerDto.Email))
                return false;

            // Check if voter ID already exists
            if (await _context.Users.AnyAsync(u => u.VoterIdNumber == registerDto.VoterIdNumber))
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

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RegisterCandidateAsync(RegisterCandidateDto registerDto)
        {
            // Check if email already exists
            if (await _context.Users.AnyAsync(u => u.Email == registerDto.Email))
                return false;

            // Check if voter ID already exists
            if (await _context.Users.AnyAsync(u => u.VoterIdNumber == registerDto.VoterIdNumber))
                return false;

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
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

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

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

                _context.Candidates.Add(candidate);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                return false;
            }
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> VerifyUserAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return false;

            user.IsVerified = true;
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
