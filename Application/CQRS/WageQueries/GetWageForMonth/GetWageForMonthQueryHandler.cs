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

namespace Application.CQRS.WageQueries.GetWageForMonth
{
    public class GetWageForMonthQueryHandler : IRequestHandler<GetWageForMonthQuery, Response<WageResponse>>
    {

        private readonly IMapper _mapper;
        private readonly ICompanyRepository _companyRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IWorkDayReposiotry _workDayRepository;
        private readonly IPaymentInfoRepository _paymentInfoRepository;
        private readonly IWageService _wageService;

        public GetWageForMonthQueryHandler(IEmployeeRepository employeeRepository,
            IWorkDayReposiotry workDayRepository, IPaymentInfoRepository paymentInfoRepository,
            ICompanyRepository companyRepository, IMapper mapper, IWageService wageService)
        {
            _employeeRepository = employeeRepository;
            _workDayRepository = workDayRepository;
            _companyRepository = companyRepository;
            _paymentInfoRepository = paymentInfoRepository;
            _mapper = mapper;
            _wageService = wageService; 
        }

        public async Task<Response<WageResponse>> Handle(GetWageForMonthQuery request, CancellationToken cancellationToken)
        {
            Response<WageResponse> response = new();

            if (!await _companyRepository.CompanyExistAsync(request.CompanyId))
            {
                return response.SetError(404, "Could not find company");
            }

            if (!await _employeeRepository.EmployeeExistsInCompanyAsync(request.EmployeeId, request.CompanyId))
            {
                return response.SetError(404, "Could not find employee");
            }

            IEnumerable<EmployeeWorkDay> workDays = 
               await _workDayRepository.GetWorkDaysForMonth(request.EmployeeId, request.Year, request.Month);

            IEnumerable<EmployeePaymentInfo> paymentInfosForMonth = await _paymentInfoRepository
                .GetValidPaymentInfosForMonth(request.EmployeeId , request.Year , request.Month);

            var wageResponse = _wageService.
                CalculateWageForMonth(workDays, paymentInfosForMonth, request.Month , request.Year , 
                request.EmployeeId);

            if(wageResponse == null)
            {
                return response.SetError(400, "Cannot find valid payment info " +
                    "contract for all month");
            }

            response.Value = wageResponse;

            return response;
        }
    }
}
