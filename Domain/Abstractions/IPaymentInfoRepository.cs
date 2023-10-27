using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Abstractions
{
    public interface IPaymentInfoRepository
    {
        Task InsertPaymentInfo(EmployeePaymentInfo paymentInfo);

        Task<EmployeePaymentInfo?> GetPaymentInfo(Guid paymentInfoId, Guid employeeId);

        Task<IEnumerable<EmployeePaymentInfo>> GetPaymentInfos(Guid employeeId); 
    }
}
