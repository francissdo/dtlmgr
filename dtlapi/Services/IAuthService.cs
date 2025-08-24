using dtlapi.Models;

namespace dtlapi.Services
{
    public interface IAuthService
    {
        Task<AuthResponse?> LoginAsync(LoginRequest request);
        string GenerateJwtToken(User user);
    }
}
