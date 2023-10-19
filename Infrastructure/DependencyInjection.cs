using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{

    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastucture(this IServiceCollection services)
        {
            services.AddDbContext<HrMeContext>(dbContextOptions =>
            {
                dbContextOptions.UseSqlServer("Server=.;Database=Hrme;Trusted_Connection=True;TrustServerCertificate=True;");
            });

            return services;
        }
    }
}
