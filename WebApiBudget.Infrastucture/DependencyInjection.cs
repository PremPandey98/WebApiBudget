using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebApiBudget.Application.Authentication.Interfaces;
using WebApiBudget.DomainOrCore.Interfaces;
using WebApiBudget.DomainOrCore.Models;
using WebApiBudget.Infrastucture.Authentication;
using WebApiBudget.Infrastucture.Repositories;
using WebApiBudget.Infrastucture.Services;

namespace WebApiBudget.Infrastucture
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastuctureDI(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<Data.AppDbContext>(options =>
            {
               options.UseSqlServer(
                   configuration.GetConnectionString("DefaultConnection"),
                   sqlOptions => sqlOptions.EnableRetryOnFailure()
               );
            }); 
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
            
            // Add Memory Cache for OTP storage
            services.AddMemoryCache();
            
            // Register MediatR for Infrastructure handlers only (Application already registers for commands)
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));
            
            services.AddScoped<IUsersRepository, UserRepository>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ITokenBlacklistRepository, TokenBlacklistRepository>();
            services.AddSingleton<TokenValidatorService>();
            services.AddScoped<IGroupRepository, GroupRepository>();
            services.AddScoped<IExpenseRecordsRepository, ExpenseRecordsRepository>();
            services.AddScoped<IExpenseCategoryRepository, ExpenseCategoryRepository>();
            services.AddScoped<IDepositRepository, DepositRepository>();
            
            // Email and OTP services (using cache-based OTP service, no database repository needed)
            services.AddScoped<IOtpService, CacheBasedOtpService>();
            services.AddScoped<IEmailService, EmailService>();

            return services;
        }
    }
}
