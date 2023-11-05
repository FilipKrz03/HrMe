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
    public class WorkDayRepository : BaseRepository<EmployeeWorkDay>, IWorkDayReposiotry
    {

        private readonly IPropertyMappingService _propertyMappingService;

        public WorkDayRepository
            (HrMeContext context, IPropertyMappingService propertyMappingService) : base(context)
        {
            _propertyMappingService = propertyMappingService;
        }
        public async Task<EmployeeWorkDay?> GetWorkDayAsync(Guid workDayId, Guid employeeId)
        {
            return await GetByIdQuery(workDayId)
                .Where(w => w.EmployeeId == employeeId)
                .FirstOrDefaultAsync();
        }

        public async Task<IPagedList<EmployeeWorkDay>>
            GetWorkDaysAsync(Guid employeeId, ResourceParameters resourceParameters)
        {
            var query = Query;

            var mappings =
            _propertyMappingService.GetPropertyMapping<EmployeeWorkDay, WorkDayResponse>();

            if (!resourceParameters.SearchQuery.IsNullOrEmpty())
            {
                query = query.Where(w =>
                w.Id.ToString().Contains(resourceParameters.SearchQuery!)
                || w.WorkDayDate.ToString().Contains(resourceParameters.SearchQuery!)
                );
            }

            if (!resourceParameters.OrderBy.IsNullOrEmpty())
            {
                query = IQueraybleExtensions.ApplySort(query, resourceParameters.OrderBy!, mappings);
            }

            return await PagedList<EmployeeWorkDay>
                .CreateAsync(query
                .Where(w => w.EmployeeId == employeeId),
                resourceParameters.PageNumber, resourceParameters.PageSize);
        }

        public async Task InsertWorkDay(EmployeeWorkDay employeeWorkDay)
        {
            await Insert(employeeWorkDay);
        }

        public async Task<bool> WorkDayExistAsync(DateTime workDayDate, Guid employeeId)
        {
            return await Query
            .AnyAsync
            (w => w.WorkDayDate.Day == workDayDate.Day
            && w.WorkDayDate.Month == workDayDate.Month
            && w.WorkDayDate.Year == workDayDate.Year
            && w.EmployeeId == employeeId);
        }

        public async Task DeleteWorkDayAsync(EmployeeWorkDay employeeWorkDay)
        {
            await DeleteEntity(employeeWorkDay);
        }

        async Task IWorkDayReposiotry.SaveChangesAsync()
        {
            await SaveChangesAsync();
        }

        public async Task<bool> OtherWorkDayExist(DateTime workDayDate, Guid employeeId , Guid workDayId)
        {
            return await Query
                .AnyAsync(w => w.WorkDayDate == workDayDate
                && w.EmployeeId == employeeId
                && w.Id != workDayId);
        }
    }
}
