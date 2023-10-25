using Application.CQRS.PaymentInfo.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.PaymentInfo.Query.GetPaymentInfos
{
    public class GetPaymentInfosQuery : IRequest<Response<IEnumerable<PaymentInfoResponse>>>
    {
        public Guid CompanyId { get; set; } 

        public Guid EmployeeId { get; set; }

        public GetPaymentInfosQuery(Guid companyId , Guid employeeId)
        {
            CompanyId = companyId;
            EmployeeId = employeeId;
        }
    }
}
