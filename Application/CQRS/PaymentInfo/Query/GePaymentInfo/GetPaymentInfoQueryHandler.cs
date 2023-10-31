using Domain.Responses;
using AutoMapper;
using Azure;
using Domain.Abstractions;
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

        private readonly IMapper _mapper;
        private readonly ICompanyRepository _companyRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IPaymentInfoRepository _paymentInfoRepository;

        public GetPaymentInfoQueryHandler(IMapper mapper, ICompanyRepository companyRepository,
            IEmployeeRepository employeeRepository, IPaymentInfoRepository paymentInfoRepository)
        {
            _mapper = mapper;
            _companyRepository = companyRepository;
            _employeeRepository = employeeRepository;
            _paymentInfoRepository = paymentInfoRepository;
        }

        public async Task<Response<PaymentInfoResponse>> Handle(GetPaymentInfoQuery request, CancellationToken cancellationToken)
        {
            Response<PaymentInfoResponse> response = new();

            var companyExist = await _companyRepository.CompanyExistAsync(request.CompanyId);

            if (!companyExist)
            {
                return response.SetError(404, "We could not find your company");
            }

            var employeeExist = await _employeeRepository
                .EmployeeExistsInCompanyAsync(request.EmployeeId, request.CompanyId);

            if (!employeeExist)
            {
                return response.SetError(404, $"We could not find employee with id {request.EmployeeId}");
            }

            var paymentInfo = await _paymentInfoRepository
                .GetPaymentInfo(request.PaymentId, request.EmployeeId);

            if (paymentInfo == null)
            {
                return response.SetError(404, "We could not find  payment info");
            }

            response.Value = _mapper.Map<PaymentInfoResponse>(paymentInfo);

            return response;
        }
    }
}
