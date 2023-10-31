using Domain.Abstractions;
using Domain.Common;
using Domain.Entities;
using Domain.Responses;
using Infrastructure.Common;
using Infrastructure.PropertyMapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
    {

        private readonly IPropertyMappingService _propertyMappingService;
        public EmployeeRepository(HrMeContext context, IPropertyMappingService propertyMappingService) : base(context)
        {
            _propertyMappingService = propertyMappingService;
        }

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
            var query = Query;

            var mappings =
             _propertyMappingService.GetPropertyMapping<Employee, EmployeeResponse>();

            if (!resourceParameters.SearchQuery.IsNullOrEmpty())
            {
                query = query.Where
                    (e => e.FirstName.Contains(resourceParameters.SearchQuery!)
                    || e.LastName.Contains(resourceParameters.SearchQuery!)
                    || e.Id.ToString().Contains(resourceParameters.SearchQuery!)
                    || e.Email.Contains(resourceParameters.SearchQuery!)
                    || e.Position.Contains(resourceParameters.SearchQuery!));
            }

            if (!resourceParameters.OrderBy.IsNullOrEmpty())
            {
                query = IQueraybleExtensions.ApplySort(query, resourceParameters.OrderBy!, mappings);
            }

            return await PagedList<Employee>
                .CreateAsync(query
                 .Where(e => e.CompanyId == companyId),
                 resourceParameters.PageNumber, resourceParameters.PageSize);
        }

        public async Task InsertEmployee(Employee employee)
        {
            await Insert(employee);
        }
    }
}
