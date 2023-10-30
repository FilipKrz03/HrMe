using Application.CQRS.PaymentInfo.Response;
using Domain.Abstractions;
using Domain.Common;
using Infrastructure.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.PaymentInfo.Query.GetPaymentInfos
{
    public class GetPaymentInfosQuery : IRequest<Response<PagedList<PaymentInfoResponse>>>
    {
        public Guid CompanyId { get; set; } 

        public Guid EmployeeId { get; set; }

        public ResourceParameters ResourceParameters { get; set; }

        public GetPaymentInfosQuery
            (Guid companyId , Guid employeeId , ResourceParameters resourceParameters)
        {
            CompanyId = companyId;
            EmployeeId = employeeId;
            ResourceParameters = resourceParameters;
        }
    }
}
