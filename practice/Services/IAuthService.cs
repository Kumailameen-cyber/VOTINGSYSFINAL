using practice.DTOs;
using practice.Models;

namespace practice.Services
{
    public interface IAuthService
    {
        Task<LoginResponseDto?> LoginAsync(LoginDto loginDto);
        Task<bool> RegisterVoterAsync(RegisterVoterDto registerDto);
        Task<bool> RegisterCandidateAsync(RegisterCandidateDto registerDto);
        Task<User?> GetUserByEmailAsync(string email);
        Task<bool> VerifyUserAsync(int userId);
    }
}
