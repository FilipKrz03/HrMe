using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class HrMeContextFactory : IDesignTimeDbContextFactory<HrMeContext>
    {
        HrMeContext IDesignTimeDbContextFactory<HrMeContext>.CreateDbContext(string[] args)
        {
            IConfigurationRoot _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<HrMeContext>();
            var conString = Environment.GetEnvironmentVariable("ConnectionString");

            optionsBuilder.UseSqlServer(conString);

            return new HrMeContext(optionsBuilder.Options);
        }
    }
}
