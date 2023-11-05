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
    }
}
