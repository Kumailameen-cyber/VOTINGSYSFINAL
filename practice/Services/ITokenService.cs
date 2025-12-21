using practice.Models;

namespace practice.Services
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
