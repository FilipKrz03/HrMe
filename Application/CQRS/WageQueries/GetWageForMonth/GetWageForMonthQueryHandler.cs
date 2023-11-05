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
        private readonly ICompanyRepository _companyRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IWageService _wageService;

        public GetWageForMonthQueryHandler(IEmployeeRepository employeeRepository,
            ICompanyRepository companyRepository, IWageService wageService)
        {
            _employeeRepository = employeeRepository;
            _companyRepository = companyRepository;
            _wageService = wageService;
        }

        public async Task<Response<WageResponse>> Handle(GetWageForMonthQuery request, CancellationToken cancellationToken)
        {
            Response<WageResponse> response = new();

            if (!await _companyRepository.CompanyExistAsync(request.CompanyId))
            {
                return response.SetError(404, "Could not find company");
            }

            var employee = await _employeeRepository
                .GetEmployeeWithPaymentDataForMonth
                (request.CompanyId, request.EmployeeId, request.Year, request.Month);

            if (employee == null)
            {
                return response.SetError(404, "Could not find employee");
            }

            var wageResponse = _wageService.
                CalculateWageForMonth(employee.WorkDays, employee.PaymentInfos, request.Month, request.Year,
                request.EmployeeId);

            if (wageResponse == null)
            {
                return response.SetError(400, "Cannot find valid payment info " +
                    "contract for all month");
            }

            response.Value = wageResponse;

            return response;
        }
    }
}
