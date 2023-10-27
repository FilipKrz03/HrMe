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

        Task<IEnumerable<EmployeeWorkDay>> GetWorkDaysAsync(Guid employeeId);
    }
}
