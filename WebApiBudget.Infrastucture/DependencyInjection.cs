using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebApiBudget.Application.Authentication.Interfaces;
using WebApiBudget.DomainOrCore.Interfaces;
using WebApiBudget.DomainOrCore.Models;
using WebApiBudget.Infrastucture.Authentication;
using WebApiBudget.Infrastucture.Repositories;

namespace WebApiBudget.Infrastucture
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastuctureDI(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<Data.AppDbContext>(options =>
            {
               options.UseSqlServer("Server=localhost;Database=BudgetDB;Trusted_Connection=True;TrustServerCertificate=true;MultipleActiveResultSets=true");
            }); 
            
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
            services.AddScoped<IUsersRepository, UserRepository>();
            services.AddScoped<IAuthService, AuthService>();
            
            return services;
        }
    }
}
