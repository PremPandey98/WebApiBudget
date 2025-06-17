using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApiBudget.Application.Authentication.Interfaces;
using WebApiBudget.DomainOrCore.Entities;
using WebApiBudget.DomainOrCore.Models;
using WebApiBudget.Infrastucture.Data;

namespace WebApiBudget.Infrastucture.Authentication
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly JwtSettings _jwtSettings;

        public AuthService(AppDbContext context, IOptions<JwtSettings> jwtSettings)
        {
            _context = context;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<string> GenerateTokenAsync(UsersEntity user)
        {
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, ((UserRole)user.Role).ToString())
            };

            var tokenExpiration = DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes);
            
            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: tokenExpiration,
                signingCredentials: signingCredentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<UsersEntity?> ValidateUserAsync(string username, string password)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserName.ToLower() == username.ToLower() && u.IsActive);

            if (user != null && user.Password == password)  // In production, use password hashing!
                return user;

            return null;
        }
    }
}
