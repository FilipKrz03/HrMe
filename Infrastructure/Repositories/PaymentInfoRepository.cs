using Domain.Abstractions;
using Domain.Common;
using Domain.Entities;
using Infrastructure.Common;
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

        public async Task<IPagedList<EmployeePaymentInfo>>
            GetPaymentInfos(Guid employeeId , ResourceParameters resourceParameters)
        {
            return await PagedList<EmployeePaymentInfo>
                .CreateAsync(Query
                .Where(p => p.EmployeeId == employeeId),
                resourceParameters.PageNumber, resourceParameters.PageSize);
        }

        public async Task InsertPaymentInfo(EmployeePaymentInfo paymentInfo)
        {
            await Insert(paymentInfo);
        }
    }
}
