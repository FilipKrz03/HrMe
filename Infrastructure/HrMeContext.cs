﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Infrastructure
{
    public class HrMeContext : DbContext
    {
        public DbSet<Company> Companies { get; set; }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<EmployeeWorkDay> EmployeesWorkDays { get; set; }

        public DbSet<EmployeePaymentInfo> EmployeesPaymentInfos {  get; set; }  

        public DbSet<EmployeeMonthlyBonus> EmployeesMonthlyBonuses {  get; set; }   

        public HrMeContext(DbContextOptions<HrMeContext> options) : base(options) { }
    }
}
