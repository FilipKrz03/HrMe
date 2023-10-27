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
    public class WorkDayRepository : BaseRepository<EmployeeWorkDay>, IWorkDayReposiotry
    {

        public WorkDayRepository(HrMeContext context) : base(context) { }

        public async Task<EmployeeWorkDay?> GetWorkDayAsync(Guid workDayId, Guid employeeId)
        {
            return await GetByIdQuery(workDayId)
                .Where(w => w.EmployeeId == employeeId)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<EmployeeWorkDay>> GetWorkDaysAsync(Guid employeeId)
        {
            return await Query
                .Where(w => w.EmployeeId == employeeId)
                .ToListAsync();
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
    }
}
