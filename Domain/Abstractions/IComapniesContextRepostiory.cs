using Domain.Common.Responses;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Abstractions
{
    public interface IComapniesContextRepostiory
    {
        public Task<bool> CompanyExistAsync(Guid companyId);

        public Task<EmployeAndCompanyExist>
            EmployeAndCompanyExistAsync(Guid companyId, Guid employeeId);

        public Task SaveChangesAsync();
        public void CreateWorkDay(Guid employeeId, EmployeeWorkDay employeeWorkDay);

        public Task<bool> EmployeeWorkDayExistAsync(DateTime workDayDate, Guid employeeId);

        public Task<EmployeeWorkDay?> GetEmployeeWorkDayAsync(Guid employeeId, Guid workDayId);

        public Task<IEnumerable<EmployeeWorkDay>> GetEmployeeWorkDaysAsync(Guid employeeId);

    }
}
