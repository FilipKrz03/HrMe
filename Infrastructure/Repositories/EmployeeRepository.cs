using Domain.Abstractions;
using Domain.Common;
using Domain.Entities;
using Infrastructure.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(HrMeContext context) : base(context) { }

        public async Task<bool> EmployeeExistAsync(Guid employeeId)
        {
            return await EntityExistAsync(employeeId);
        }

        public async Task<bool> EmployeeExistsInCompanyAsync(Guid employeeId, Guid companyId)
        {
            return await GetByIdQuery(employeeId)
                .AnyAsync(e => e.CompanyId == companyId);
        }

        public async Task<bool> EmployeExistWithEmailInCompanyAsync(string email, Guid companyId)
        {
            return await Query
                .AnyAsync(e => e.Email == email && e.CompanyId == companyId);
        }

        public async Task<Employee?> GetEmployeeAsync(Guid employeeId, Guid companyId)
        {
            return await GetByIdQuery(employeeId)
                .Where(e => e.CompanyId == companyId)
                .Include(e => e.PaymentInfos)
                .Include(e => e.WorkDays)
                .FirstOrDefaultAsync();
        }

        public async Task<IPagedList<Employee>> 
            GetEmployeesAsync(Guid companyId, ResourceParameters resourceParameters)
        {
            return await PagedList<Employee>
                .CreateAsync(Query
                 .Where(e => e.CompanyId == companyId),
                 resourceParameters.PageNumber, resourceParameters.PageSize);
        }

        public async Task InsertEmployee(Employee employee)
        {
            await Insert(employee);
        }
    }
}
