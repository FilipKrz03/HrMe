using Azure.Core;
using Domain.Abstractions;
using Domain.Common.Responses;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ComapniesContexRepostiory : IComapniesContextRepostiory
    {
        private readonly HrMeContext _context;
        public ComapniesContexRepostiory(HrMeContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> CompanyExistAsync(Guid companyId)
        {
            var companyExist = await _context
                .Companies
                .AnyAsync(c => c.Id == companyId);

            return companyExist;
        }

        public async Task<bool> EmployeeWorkDayExistAsync(DateTime workDayDate, Guid employeeId)
        {
            var workDayExist = await _context.EmployeesWorkDays
            .AnyAsync
            (w => w.WorkDayDate.Day == workDayDate.Day
            && w.WorkDayDate.Month == workDayDate.Month
            && w.WorkDayDate.Year == workDayDate.Year
            && w.EmployeeId == employeeId);

            return workDayExist;
        }

        public async Task<EmployeAndCompanyExist> EmployeAndCompanyExistAsync(Guid companyId, Guid employeeId)
        {
            EmployeAndCompanyExist employeAndCompanyExist = new();

            var compnayExist = await CompanyExistAsync(companyId);

            if (!compnayExist)
            {
                employeAndCompanyExist.CompanyExist = false;
                employeAndCompanyExist.EmployeeExist = false;
                return employeAndCompanyExist;
            }

            var employeeExist = await _context
                .Employees
                .AnyAsync(e => e.Id == employeeId && e.CompanyId == companyId);

            if (!employeeExist)
            {
                employeAndCompanyExist.CompanyExist = true;
                employeAndCompanyExist.EmployeeExist = false;
                return employeAndCompanyExist;
            }

            return employeAndCompanyExist;
        }

        public void CreateWorkDay(Guid employeeId, EmployeeWorkDay employeeWorkDay)
        {
            employeeWorkDay.EmployeeId = employeeId;

            _context.EmployeesWorkDays.Add(employeeWorkDay);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<EmployeeWorkDay?> GetEmployeeWorkDayAsync(Guid employeeId , Guid workDayId)
        {
            return await _context.EmployeesWorkDays
                .Where(w => w.Id == workDayId && w.EmployeeId == employeeId)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<EmployeeWorkDay>> GetEmployeeWorkDaysAsync(Guid employeeId)
        {
            return await _context.EmployeesWorkDays
                .Where(w => w.EmployeeId == employeeId)
                .ToListAsync();
        }
    }
}
