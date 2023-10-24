using Application.CQRS.PaymentInfo.Response;
using Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.PaymentInfo.Command.CreatePaymentInfo
{
    public class CreatePaymentInfoCommand : IRequest<Response<PaymentInfoResponse>>
    {
        public Guid CompanyId { get; set; }

        public Guid EmployeeId { get; set; }    

        public double HourlyRateBrutto { get; set; }

        public ContractType ContractType { get; set; }

        public DateTime StartOfContractDate { get; set; }

        public DateTime? EndOfContractDate { get; set; }

        public CreatePaymentInfoCommand
            (Guid companyId , Guid employeeId , double hourlyRateBrutto , ContractType contractType , 
            DateTime startOfContractDate , DateTime? endOfcontractDate)
        {
            CompanyId = companyId;
            EmployeeId = employeeId;
            HourlyRateBrutto = hourlyRateBrutto;
            ContractType = contractType;
            StartOfContractDate = startOfContractDate;

            if(endOfcontractDate != null)
            {
                EndOfContractDate = endOfcontractDate;  
            }
        }
    }
}
