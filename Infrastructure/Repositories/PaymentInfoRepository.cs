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
    public class PaymentInfoRepository : BaseRepository<EmployeePaymentInfo>, IPaymentInfoRepository
    {
        public PaymentInfoRepository(HrMeContext context) : base(context) { }

        public async Task<EmployeePaymentInfo?> GetPaymentInfo(Guid paymentInfoId, Guid employeeId)
        {
            return await GetByIdQuery(paymentInfoId)
                 .Where(p => p.EmployeeId == employeeId)
                 .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<EmployeePaymentInfo>> GetPaymentInfos(Guid employeeId)
        {
            return await Query
                .Where(p => p.EmployeeId == employeeId)
                .ToListAsync();
        }

        public async Task InsertPaymentInfo(EmployeePaymentInfo paymentInfo)
        {
            await Insert(paymentInfo);
        }
    }
}
