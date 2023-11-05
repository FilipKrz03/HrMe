using Domain.Abstractions;
using Infrastructure.Authentiaction;
using Infrastructure.Repositories;
using Infrastructure.PropertyMapping;
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

            services.AddScoped<ICompanyRepository, CompanyRepostiory>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IWorkDayReposiotry, WorkDayRepository>();
            services.AddScoped<IPaymentInfoRepository, PaymentInfoRepository>();
            services.AddScoped<IMonthlyBonusRepository, MonthlyBonusRepository>();

            services.AddTransient<IPropertyMappingService ,PropertyMappingService>();

            return services;
        }
    }
}
