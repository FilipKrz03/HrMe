using Domain.Abstractions;
using Domain.Entities;
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

        public async Task InsertMonthlyBonus(EmployeeMonthlyBonus monthlyBonus)
        {
            await Insert(monthlyBonus);
        }

    }
}
