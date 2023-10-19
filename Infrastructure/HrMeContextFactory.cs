using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class HrMeContextFactory : IDesignTimeDbContextFactory<HrMeContext>  
    {
        public HrMeContext CreateDbContext(string[] args)
        {
            
            var optionsBuilder = new DbContextOptionsBuilder<HrMeContext>();
            optionsBuilder.UseSqlServer("Server=.;Database=Hrme;Trusted_Connection=True;TrustServerCertificate=True;");

            return new HrMeContext(optionsBuilder.Options);
        }
    }
}
