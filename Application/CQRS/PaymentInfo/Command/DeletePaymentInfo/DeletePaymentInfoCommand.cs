using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.PaymentInfo.Command.DeletePaymentInfo
{
    public class DeletePaymentInfoCommand : IRequest<Response<bool>>
    {
        public Guid CompanyId { get; set; }

        public Guid EmployeeId { get; set; }

        public Guid PaymentInfoId {  get; set; }

        public DeletePaymentInfoCommand(Guid companyId, Guid employeeId , 
            Guid paymentInfoId)
        {
            CompanyId = companyId;
            EmployeeId = employeeId;
            PaymentInfoId = paymentInfoId;
        }

    }
}
