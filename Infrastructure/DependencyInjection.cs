using Domain.Abstractions;
using Infrastructure.Authentiaction;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
        public static IServiceCollection AddInfrastucture(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<HrMeContext>(dbContextOptions =>
            {
                var conectionString = configuration["ConnectionStrings:DefaultConnection"];
                dbContextOptions.UseSqlServer(conectionString);
            });

            services.AddSingleton<IJwtProvider, JwtProvider>();

            services.AddScoped<IComapniesContextRepostiory, ComapniesContexRepostiory>();

            return services;
        }
    }
}
