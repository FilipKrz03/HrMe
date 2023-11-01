using Application.Common;
using AutoMapper;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.PaymentInfo.Command.PutPaymentInfo
{
    public class PutPaymentInfoCommandHandler : IRequestHandler<PutPaymentInfoCommand, Response<PaymentInfoResponse>>
    {

        private readonly IMapper _mapper;
        private readonly ICompanyRepository _companyRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IPaymentInfoRepository _paymentInfoRepository;

        public PutPaymentInfoCommandHandler(IMapper mapper, IEmployeeRepository employeeRepository,
            ICompanyRepository companyRepository, IPaymentInfoRepository paymentInfoRepository)
        {
            _mapper = mapper;
            _employeeRepository = employeeRepository;
            _companyRepository = companyRepository;
            _paymentInfoRepository = paymentInfoRepository;
        }
        public async Task<Response<PaymentInfoResponse>> Handle(PutPaymentInfoCommand request, CancellationToken cancellationToken)
        {
            Response<PaymentInfoResponse> response = new();

            if (!await _companyRepository.CompanyExistAsync(request.CompanyId))
            {
                return response.SetError(404, "We could not find company");
            }

            if(!await _employeeRepository.EmployeeExistsInCompanyAsync(request.EmployeeId,request.CompanyId))
            {
                return response.SetError(404, "We could not find employee");
            }

            var paymentInfo = await 
                _paymentInfoRepository.GetPaymentInfo(request.PaymentInfoId, request.EmployeeId);

            if(paymentInfo == null)
            {
                bool isDataNotAvaliable = await _paymentInfoRepository
                    .ContractDateIsNotAvaliableAsync(request.StartOfContractDate , request.EndOfContractDate);

                if(isDataNotAvaliable)
                {
                    return response.SetError(409, "Employee has already pending contract on current data");
                }

                EmployeePaymentInfo paymentInfoEntity = _mapper.Map<EmployeePaymentInfo>(request);

                paymentInfoEntity.EmployeeId = request.EmployeeId;
                paymentInfoEntity.Id = request.PaymentInfoId;

                await _paymentInfoRepository.InsertPaymentInfo(paymentInfoEntity);

                response.Value = _mapper.Map<PaymentInfoResponse>(paymentInfoEntity);

                return response;
            }

            bool otherContractIsPending = await _paymentInfoRepository
                .OtherContractIsPending(request.StartOfContractDate, request.EndOfContractDate, request.PaymentInfoId);

            if(otherContractIsPending)
            {
                return response.SetError(409, "Other contract is alerady pending on date that you want to change");
            }

            _mapper.Map(request, paymentInfo);

            await _employeeRepository.SaveChangesAsync();

            return response;
        }
    }
}
