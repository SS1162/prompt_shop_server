using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DTO;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _config;

        public JwtService(IConfiguration config) => _config = config;

        public string GenerateToken(UserDTO user, string username, string password)
        {
            bool isAdmin = string.Equals(username, _config["AdminName"], StringComparison.OrdinalIgnoreCase)
                        && password == _config["AdminPassword"];

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"]!));
            var now = DateTime.UtcNow;

            var descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserID.ToString()),
                    new Claim(ClaimTypes.Role, isAdmin ? "admin" : "user"),
                    new Claim(JwtRegisteredClaimNames.Iat,
                        new DateTimeOffset(now).ToUnixTimeSeconds().ToString(),
                        ClaimValueTypes.Integer64),
                }),
                Issuer             = _config["Jwt:Issuer"],
                Audience           = _config["Jwt:Audience"],
                Expires            = now.AddHours(int.Parse(_config["Jwt:ExpiresHours"] ?? "6")),
                SigningCredentials  = new SigningCredentials(key, SecurityAlgorithms.HmacSha256),
            };

            var handler = new JwtSecurityTokenHandler();
            return handler.WriteToken(handler.CreateToken(descriptor));
        }
    }
}
