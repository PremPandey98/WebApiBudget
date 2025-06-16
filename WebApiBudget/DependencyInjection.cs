using WebApiBudget.Application;
using WebApiBudget.Infrastucture;

namespace WebApiBudget
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAppDI(this IServiceCollection services)
        {
            services.AddApplicationDI().AddInfrastuctureDI();
            return services;
        }
    }
}
