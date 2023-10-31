using Application.Common;
using Domain.Responses;
using AutoMapper;
using Domain.Abstractions;
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

        private readonly IMapper _mapper;
        private readonly ICompanyRepository _companyRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IPaymentInfoRepository _paymentInfoRepository;

        public CreatePaymentInfoCommandHandler(IMapper mapper , IEmployeeRepository employeeRepository , 
            ICompanyRepository companyRepository , IPaymentInfoRepository paymentInfoRepository )
        {
            _mapper = mapper;
            _employeeRepository = employeeRepository;
            _companyRepository = companyRepository;
            _paymentInfoRepository = paymentInfoRepository;
        }

        public async Task<Response<PaymentInfoResponse>>
            Handle(CreatePaymentInfoCommand request, CancellationToken cancellationToken)
        {
            Response<PaymentInfoResponse> response = new();

            var companyExist = await _companyRepository.CompanyExistAsync(request.CompanyId);   

            if (!companyExist)
            {
                return response.SetError(404, "We could not find your company");
            }

            var employee = await _employeeRepository.GetEmployeeAsync(request.EmployeeId , request.CompanyId);

            if (employee == null)
            {
                return response.SetError(404, $"We could not find employee with id {request.EmployeeId}");
            }

            bool isPaymantInfoDayRangeAvaliable =
                 DateTimeExtensions.IsPaymentInfoDateAvaliable
                 (request.StartOfContractDate, request.EndOfContractDate, employee.PaymentInfos);

            if (isPaymantInfoDayRangeAvaliable == false)
            {
                return response.SetError(409, "Employee has already contract on requested time range");
            }

            EmployeePaymentInfo paymentInfoEntity = _mapper.Map<EmployeePaymentInfo>(request);

            paymentInfoEntity.EmployeeId = request.EmployeeId;

            await _paymentInfoRepository.InsertPaymentInfo(paymentInfoEntity);

            response.Value = _mapper.Map<PaymentInfoResponse>(paymentInfoEntity);

            return response;
        }
    }
}
