using AutoMapper;
using Domain.Abstractions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.PaymentInfo.Command.DeletePaymentInfo
{
    public class DeletePaymentInfoCommandHandler : IRequestHandler<DeletePaymentInfoCommand, Response<bool>>
    {

        private readonly ICompanyRepository _companyRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IPaymentInfoRepository _paymentInfoRepository;

        public DeletePaymentInfoCommandHandler( IEmployeeRepository employeeRepository,
            ICompanyRepository companyRepository, IPaymentInfoRepository paymentInfoRepository)
        {
            _employeeRepository = employeeRepository;
            _companyRepository = companyRepository;
            _paymentInfoRepository = paymentInfoRepository;
        }
        public async Task<Response<bool>> Handle(DeletePaymentInfoCommand request, CancellationToken cancellationToken)
        {
            Response<bool> response = new();

            if(! await _companyRepository.CompanyExistAsync(request.CompanyId))
            {
                return response.SetError(404 , "Company does not exist");
            }

            if(! await _employeeRepository.EmployeeExistsInCompanyAsync(request.EmployeeId , request.CompanyId))
            {
                return response.SetError(404, "Employee does not exist");
            }

            var paymentInfo = 
                await _paymentInfoRepository.GetPaymentInfo(request.PaymentInfoId, request.EmployeeId);

            if(paymentInfo == null)
            {
                return response.SetError(404, "Payment info does not exist");
            }

            await _paymentInfoRepository.DeletePaymentInfo(paymentInfo);

            response.Value = true;

            return response;
        }
    }
}
