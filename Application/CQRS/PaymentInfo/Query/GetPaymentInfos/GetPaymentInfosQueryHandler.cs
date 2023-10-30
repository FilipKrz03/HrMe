using Application.CQRS.PaymentInfo.Response;
using AutoMapper;
using Azure;
using Domain.Abstractions;
using Infrastructure;
using Infrastructure.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.PaymentInfo.Query.GetPaymentInfos
{
    public class GetPaymentInfosQueryHandler : IRequestHandler<GetPaymentInfosQuery, Response<PagedList<PaymentInfoResponse>>>
    {

        private readonly IMapper _mapper;
        private readonly IPaymentInfoRepository _paymentInfoRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public GetPaymentInfosQueryHandler(IMapper mapper, ICompanyRepository companyRepository,
            IEmployeeRepository employeeRepository , IPaymentInfoRepository paymentInfoRepository)
        {
            _mapper = mapper;
            _companyRepository = companyRepository;
            _employeeRepository = employeeRepository;
            _paymentInfoRepository = paymentInfoRepository;
        }

        public async Task<Response<PagedList<PaymentInfoResponse>>>
            Handle(GetPaymentInfosQuery request, CancellationToken cancellationToken)
        {
            Response<PagedList<PaymentInfoResponse>> response = new();

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

            var paymentInfos = await _paymentInfoRepository.GetPaymentInfos(request.EmployeeId , request.ResourceParameters);

            response.Value = _mapper.Map<PagedList<PaymentInfoResponse>>(paymentInfos);

            return response;
        }
    }
}
