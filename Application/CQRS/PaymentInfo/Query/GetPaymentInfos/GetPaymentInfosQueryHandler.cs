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

namespace Application.CQRS.PaymentInfo.Query.GetPaymentInfos
{
    public class GetPaymentInfosQueryHandler : IRequestHandler<GetPaymentInfosQuery, Response<IEnumerable<PaymentInfoResponse>>>
    {

        private readonly HrMeContext _context;
        private readonly IMapper _mapper;

        public GetPaymentInfosQueryHandler(HrMeContext context, IMapper mapper)
        {
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
            _context = context ??
                throw new ArgumentNullException(nameof(context));
        }


        public async Task<Response<IEnumerable<PaymentInfoResponse>>> Handle(GetPaymentInfosQuery request, CancellationToken cancellationToken)
        {
            Response<IEnumerable<PaymentInfoResponse>> response = new();

            var companyExist = await _context
            .Companies
            .AnyAsync(c => c.Id == request.CompanyId, cancellationToken);

            if (!companyExist)
            {
                response.SetError(404, "We could not find your company");
                return response;
            }

            var employeeExist = await _context
                .Employees
                .AnyAsync(e => e.Id == request.EmployeeId && e.CompanyId == request.CompanyId, cancellationToken);


            if (!employeeExist)
            {
                response.SetError(404, $"We could not find employee with id {request.EmployeeId}");
                return response;
            }


            var paymentInfos =
                await _context.EmployeesPaymentInfos
                .Where(p => p.EmployeeId == request.EmployeeId)
                .ToListAsync(cancellationToken);

            response.Value = _mapper.Map<IEnumerable<PaymentInfoResponse>>(paymentInfos);

            return response;
        }
    }
}
