using Application.CQRS.PaymentInfo.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.PaymentInfo.Query.GePaymentInfo
{
    public class GetPaymentInfoQuery : IRequest<Response<PaymentInfoResponse>>
    {
        public Guid CompanyId {  get; set; }    
        
        public Guid EmployeeId { get; set; }

        public Guid PaymentId { get; set; }


        public GetPaymentInfoQuery(Guid companyId , Guid employeeId , 
            Guid paymentId)
        {
            CompanyId = companyId;
            EmployeeId = employeeId;
            PaymentId = paymentId;
        }
    }
}
