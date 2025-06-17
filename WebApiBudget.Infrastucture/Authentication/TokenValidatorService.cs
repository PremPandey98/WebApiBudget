using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using WebApiBudget.DomainOrCore.Interfaces;

namespace WebApiBudget.Infrastucture.Authentication
{
    public class TokenValidatorService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public TokenValidatorService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }        
        public async Task ValidateAsync(TokenValidatedContext context)
        {
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var tokenBlacklistRepository = scope.ServiceProvider.GetRequiredService<ITokenBlacklistRepository>();
                
                // Extract token from Authorization header
                string tokenString = context.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                
                if (await tokenBlacklistRepository.IsTokenBlacklistedAsync(tokenString))
                {
                    // Token is blacklisted, reject it
                    context.Fail("Token has been revoked");
                }
            }
            catch (Exception ex)
            {
                // Log the error but don't reject the token
                System.Diagnostics.Debug.WriteLine($"Error validating token against blacklist: {ex.Message}");
            }
        }
    }
}
