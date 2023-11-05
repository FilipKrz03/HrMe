using Domain.Common;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Abstractions
{
    public interface IMonthlyBonusRepository
    {
        Task InsertMonthlyBonus(EmployeeMonthlyBonus monthlyBonus);

        Task<EmployeeMonthlyBonus?> GetMonthlyBonus(Guid employeeId, Guid monthlyBonusId);

        Task<IPagedList<EmployeeMonthlyBonus>> GetEmployeeMonthlyBonuses(Guid employeeId, ResourceParameters resourceParameters);

        Task<bool> EmployeMonthlyBonusExistAsync(Guid employeeId, Guid monthlyBonusId);

        Task DeleteMonthlyBonusAsync(EmployeeMonthlyBonus monthlyBonus);

        Task<bool> MonthlyBonusDateNotAvaliable(Guid employeeId, int year, int month);

        Task SaveChangesAsync();

        Task<bool> OtherMonthlyBonusDateExist(Guid employeeId, Guid monthlyBonusId, int year, int month);
    }
}
