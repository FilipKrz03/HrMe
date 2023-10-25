using Application.CQRS.PaymentInfo.Response;
using AutoMapper;
using Azure;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.PaymentInfo.Query.GePaymentInfo
{
    public class GetPaymentInfoQueryHandler : IRequestHandler<GetPaymentInfoQuery, Response<PaymentInfoResponse>>
    {

        private readonly HrMeContext _context;
        private readonly IMapper _mapper;

        public GetPaymentInfoQueryHandler(HrMeContext context, IMapper mapper)
        {
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
            _context = context ??
                throw new ArgumentNullException(nameof(context));
        }

        public async Task<Response<PaymentInfoResponse>> Handle(GetPaymentInfoQuery request, CancellationToken cancellationToken)
        {
            Response<PaymentInfoResponse> response = new();


            var companyExist = await _context
              .Companies
              .AnyAsync(c => c.Id == request.CompanyId, cancellationToken);

            if (!companyExist)
            {
                return response.SetError(404, "We could not find your company");
            }

            var employeeExist = await _context
                   .Employees
                   .AnyAsync(e => e.Id == request.EmployeeId && e.CompanyId == request.CompanyId, cancellationToken);


            if (!employeeExist)
            {
                return response.SetError(404, $"We could not find employee with id {request.EmployeeId}");
            }

            var paymentInfo = await _context.EmployeesPaymentInfos
                .Where(p => p.Id == request.PaymentId && p.EmployeeId == request.EmployeeId)
                .FirstOrDefaultAsync(cancellationToken);

            if (paymentInfo == null)
            {
                return response.SetError(404, "We could not find  payment info");
            }

            response.Value = _mapper.Map<PaymentInfoResponse>(paymentInfo);

            return response;
        }
    }
}
