using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebApiBudget.DomainOrCore.Interfaces;
using WebApiBudget.Infrastucture.Repositories;

namespace WebApiBudget.Infrastucture
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastuctureDI(this IServiceCollection services)
        {
            services.AddDbContext<Data.AppDbContext>(options =>
            {
               options.UseSqlServer("Server=localhost;Database=BudgetDB;Trusted_Connection=True;TrustServerCertificate=true;MultipleActiveResultSets=true");
            }); 
            services.AddScoped<IUsersRepository,UserRepository>();
            return services;
        }
    }
}
