using Domain.Abstractions;
using Domain.Common;
using Domain.Entities;
using Domain.Responses;
using Infrastructure.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class MonthlyBonusRepository : BaseRepository<EmployeeMonthlyBonus>, IMonthlyBonusRepository
    {
        public MonthlyBonusRepository(HrMeContext context) : base(context)
        {
        }

        public async Task<EmployeeMonthlyBonus?> GetMonthlyBonus(Guid employeeId, Guid monthlyBonusId)
        {
            return await
                Query.Where(m => m.EmployeeId == employeeId && m.Id == monthlyBonusId)
                .FirstOrDefaultAsync();
        }

        public async Task InsertMonthlyBonus(EmployeeMonthlyBonus monthlyBonus)
        {
            await Insert(monthlyBonus);
        }

        public async Task<IPagedList<EmployeeMonthlyBonus>>
            GetEmployeeMonthlyBonuses(Guid employeeId, ResourceParameters resourceParameters)
        {
            return await PagedList<EmployeeMonthlyBonus>
                .CreateAsync(Query.Where(m => m.EmployeeId == employeeId)
                , resourceParameters.PageNumber, resourceParameters.PageSize);
        }

        public async Task<bool> EmployeMonthlyBonusExistAsync(Guid employeeId , Guid monthlyBonusId)
        {
            return await Query
                .AnyAsync(b => b.Id == monthlyBonusId && b.EmployeeId == employeeId);
        }

        public async Task DeleteMonthlyBonusAsync(EmployeeMonthlyBonus monthlyBonus)
        {
            await DeleteEntity(monthlyBonus);
        }


        public async Task<bool> MonthlyBonusDateNotAvaliable(Guid employeeId , int year , int month)
        {
            return await Query
                .AnyAsync(m => m.EmployeeId == employeeId &&
                m.Year == year &&
                m.Month == month);
        }

    }
}
