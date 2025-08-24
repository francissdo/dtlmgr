using dtlapi.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace dtlapi.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly List<User> _users; // In-memory users for demo

        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
            // Initialize with demo users
            _users = new List<User>
            {
                new User
                {
                    Id = 1,
                    Username = "admin",
                    Email = "admin@example.com",
                    PasswordHash = HashPassword("admin123") // Demo password
                },
                new User
                {
                    Id = 2,
                    Username = "user",
                    Email = "user@example.com",
                    PasswordHash = HashPassword("user123") // Demo password
                }
            };
        }

        public Task<AuthResponse?> LoginAsync(LoginRequest request)
        {
            var user = _users.FirstOrDefault(u => u.Username == request.Username);
            if (user == null || !VerifyPassword(request.Password, user.PasswordHash))
            {
                return Task.FromResult<AuthResponse?>(null);
            }

            var token = GenerateJwtToken(user);
            var expiration = DateTime.UtcNow.AddHours(24);

            var response = new AuthResponse
            {
                Token = token,
                Username = user.Username,
                Expiration = expiration
            };
            
            return Task.FromResult<AuthResponse?>(response);
        }

        public string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? "your-secret-key-must-be-at-least-32-characters-long"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"] ?? "dtlapi",
                audience: _configuration["Jwt:Audience"] ?? "dtlapi-client",
                claims: claims,
                expires: DateTime.UtcNow.AddHours(24),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

        private static bool VerifyPassword(string password, string hash)
        {
            var passwordHash = HashPassword(password);
            return passwordHash == hash;
        }
    }
}
