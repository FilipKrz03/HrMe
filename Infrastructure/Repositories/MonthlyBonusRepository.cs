﻿using Domain.Abstractions;
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

    }
}
