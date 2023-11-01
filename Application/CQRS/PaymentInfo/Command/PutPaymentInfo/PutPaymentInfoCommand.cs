using Domain.Common.Enums;
using Domain.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.PaymentInfo.Command.PutPaymentInfo
{
    public class PutPaymentInfoCommand : IRequest<Response<PaymentInfoResponse>>
    {
        public Guid CompanyId { get; set; }

        public Guid EmployeeId { get; set; }

        public Guid PaymentInfoId { get; set; }

        public double HourlyRateBrutto { get; set; }

        public ContractType ContractType { get; set; }

        public DateTime StartOfContractDate { get; set; }

        public DateTime? EndOfContractDate { get; set; }

        public PutPaymentInfoCommand
            (Guid companyId, Guid employeeId, Guid paymentInfoId, double hourlyRateBrutto, ContractType contractType,
            DateTime startOfContractDate, DateTime? endOfcontractDate)
        {
            CompanyId = companyId;
            EmployeeId = employeeId;
            PaymentInfoId = paymentInfoId;
            HourlyRateBrutto = hourlyRateBrutto;
            ContractType = contractType;
            StartOfContractDate = startOfContractDate;

            if (endOfcontractDate != null)
            {
                EndOfContractDate = endOfcontractDate;
            }
        }
    }
}
