using Domain.Abstractions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class EmployeeRepository : BaseRepository<Employee> , IEmployeeRepository
    {
        public EmployeeRepository(HrMeContext context) : base(context) { }

        public async Task<bool> EmployeeExistAsync(Guid employeeId)
        {
            return await EntityExistAsync(employeeId);
        }

        public async Task<bool> EmployeExistByEmaiInCompanylAsync(string email , Guid companyId)
        {
            return await Query.
                AnyAsync(e => e.Email == email && e.CompanyId == companyId);
        }

        public Task<Employee?> GetEmployeeAsync(Guid employeeId, Guid companyId)
        {
            return GetByIdQuery(employeeId)
                .Where(e => e.CompanyId == companyId)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Employee>> GetEmployeesAsync(Guid companyId)
        {
            return await Query
                 .Where(e => e.CompanyId == companyId)
                 .ToListAsync();
        }

        public async Task InsertEmployee(Employee employee)
        {
           await Insert(employee);
        }
    }
}
