using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiBudget.DomainOrCore
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDomainOrCoreDI(this IServiceCollection services)
        {
            return services;
        }
    }
}
