using Domain.Common;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Abstractions
{
    public interface IWorkDayReposiotry
    {
        Task<bool> WorkDayExistAsync(DateTime workDayDate, Guid employeeId);

        Task InsertWorkDay(EmployeeWorkDay employeeWorkDay);

        Task<EmployeeWorkDay?> GetWorkDayAsync(Guid workDayId , Guid employeeId);

        Task<IPagedList<EmployeeWorkDay>> GetWorkDaysAsync(Guid employeeId , ResourceParameters resourceParameters);

        Task DeleteWorkDayAsync(EmployeeWorkDay employeeWorkDay);

        Task SaveChangesAsync();

        Task<bool> OtherWorkDayExist(DateTime workDayDate, Guid employeeId, Guid workDayId);
    }
}
