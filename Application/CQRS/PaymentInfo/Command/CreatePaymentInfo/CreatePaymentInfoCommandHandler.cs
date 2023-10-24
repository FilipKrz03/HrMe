using Application.CQRS.PaymentInfo.Response;
using AutoMapper;
using Domain.Entities;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.PaymentInfo.Command.CreatePaymentInfo
{
    public class CreatePaymentInfoCommandHandler : IRequestHandler<CreatePaymentInfoCommand, Response<PaymentInfoResponse>>
    {

        private readonly HrMeContext _context;
        private readonly IMapper _mapper;

        public CreatePaymentInfoCommandHandler(HrMeContext context , IMapper mapper)
        {
            _mapper = mapper ?? 
                throw new ArgumentNullException(nameof(mapper)); 
            _context = context ??
                throw new ArgumentNullException(nameof(context));
        }

        public async Task<Response<PaymentInfoResponse>>
            Handle(CreatePaymentInfoCommand request, CancellationToken cancellationToken)
        {
            Response<PaymentInfoResponse> response = new();

            var companyExist = await _context
                .Companies
                .AnyAsync(c => c.Id == request.CompanyId, cancellationToken);

            if (!companyExist)
            {
                response.SetError(404, "We could not find your company");
                return response;
            }

            var employee = await _context
                .Employees
                .Where(e => e.Id == request.EmployeeId && e.CompanyId == request.CompanyId)
                .FirstOrDefaultAsync(cancellationToken);

            if (employee == null)
            {
                response.SetError(404, $"We could not find employee with id {request.EmployeeId}");
                return response;
            }

            EmployeePaymentInfo paymentInfoEntity = _mapper.Map<EmployeePaymentInfo>(request);

            employee.PaymentInfos.Add(paymentInfoEntity);

            await _context.SaveChangesAsync(cancellationToken);

            response.Value = _mapper.Map<PaymentInfoResponse>(paymentInfoEntity);

            return response;
        }
    }
}
