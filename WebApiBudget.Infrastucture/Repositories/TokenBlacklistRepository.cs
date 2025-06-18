using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using WebApiBudget.DomainOrCore.Entities;
using WebApiBudget.DomainOrCore.Interfaces;
using WebApiBudget.Infrastucture.Data;

namespace WebApiBudget.Infrastucture.Repositories
{
    public class TokenBlacklistRepository : ITokenBlacklistRepository
    {
        private readonly AppDbContext _context;

        public TokenBlacklistRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddToBlacklistAsync(string token, Guid userId)
        {
            try
            {
                // Get the expiry time from the token
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);
                var expiry = jwtToken.ValidTo;

                var blacklistedToken = new BlacklistedTokenEntity
                {
                    Id = Guid.NewGuid(),
                    Token = token,
                    UserId = userId,
                    ExpiryTime = expiry,
                    BlacklistedAt = DateTime.UtcNow
                };

                _context.BlacklistedTokens.Add(blacklistedToken);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> IsTokenBlacklistedAsync(string token)
        {
            try
            {
                return await _context.BlacklistedTokens.AnyAsync(t => t.Token == token);
            }
            catch (Exception ex)
            {
                // Log the error but assume token is not blacklisted
                System.Diagnostics.Debug.WriteLine($"Error checking blacklist: {ex.Message}");
                return false;
            }
        }
    }
}
