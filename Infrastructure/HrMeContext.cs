using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure
{
    public class HrMeContext : DbContext
    {

        public HrMeContext(DbContextOptions<HrMeContext> dbContextOptions):base(dbContextOptions)
        {
            try
            {
                // Database Creator needed for docker only 
                var databaseCreator = Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;
                if (databaseCreator != null)
                {
                    if (!databaseCreator.CanConnect()) databaseCreator.Create();
                    if (!databaseCreator.HasTables()) databaseCreator.CreateTables();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public DbSet<Company> Companies { get; set; }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<EmployeeWorkDay> EmployeesWorkDays { get; set; }

        public DbSet<EmployeePaymentInfo> EmployeesPaymentInfos {  get; set; }  

        public DbSet<EmployeeMonthlyBonus> EmployeesMonthlyBonuses {  get; set; }   
    }
}
