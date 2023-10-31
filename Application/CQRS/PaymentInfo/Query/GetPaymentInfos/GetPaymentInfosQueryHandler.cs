using Domain.Responses;
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
using Microsoft.EntityFrameworkCore.Metadata;
using Infrastructure.PropertyMapping;

namespace Application.CQRS.PaymentInfo.Query.GetPaymentInfos
{
    public class GetPaymentInfosQueryHandler : IRequestHandler<GetPaymentInfosQuery, Response<PagedList<PaymentInfoResponse>>>
    {

        private readonly IMapper _mapper;
        private readonly IPaymentInfoRepository _paymentInfoRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IPropertyMappingService _propertyMappingService;

        public GetPaymentInfosQueryHandler(IMapper mapper, ICompanyRepository companyRepository,
            IEmployeeRepository employeeRepository , IPaymentInfoRepository paymentInfoRepository , 
            IPropertyMappingService propertyMappingService)
        {
            _mapper = mapper;
            _companyRepository = companyRepository;
            _employeeRepository = employeeRepository;
            _paymentInfoRepository = paymentInfoRepository;
            _propertyMappingService = propertyMappingService;
        }

        public async Task<Response<PagedList<PaymentInfoResponse>>>
            Handle(GetPaymentInfosQuery request, CancellationToken cancellationToken)
        {
            Response<PagedList<PaymentInfoResponse>> response = new();

            if(!_propertyMappingService.PropertyMappingExist
                <Domain.Entities.EmployeePaymentInfo , PaymentInfoResponse>(request.ResourceParameters.OrderBy!))
            {
                return response.SetError(400, $"One of orderBy Fields that you entered does not exist : " +
                     $"{request.ResourceParameters.OrderBy}");
            }

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
