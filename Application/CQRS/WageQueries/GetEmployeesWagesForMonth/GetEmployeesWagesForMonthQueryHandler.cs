using AutoMapper;
using Azure;
using Domain.Abstractions;
using Domain.Responses;
using Infrastructure.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.WageQueries.GetEmployeesWagesForMonth
{
    public class GetEmployeesWagesForMonthQueryHandler :
        IRequestHandler<GetEmployeesWagesForMonthQuery, Response<PagedList<WageResponse>>>
    {

        private readonly IMapper _mapper;
        private readonly ICompanyRepository _companyRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IWageService _wageService;

        public GetEmployeesWagesForMonthQueryHandler(IEmployeeRepository employeeRepository,
            ICompanyRepository companyRepository, IMapper mapper, IWageService wageService)
        {
            _employeeRepository = employeeRepository;
            _companyRepository = companyRepository;
            _mapper = mapper;
            _wageService = wageService;
        }

        public async Task<Response<PagedList<WageResponse>>> Handle(GetEmployeesWagesForMonthQuery request, CancellationToken cancellationToken)
        {
            Response<PagedList<WageResponse>> response = new();

            if (!await _companyRepository.CompanyExistAsync(request.CompanyId))
            {
                response.SetError(404, "We could not find your company");
            }

            var employeesWithWorkDaysAndPaymentInfoForMonth =
                await _employeeRepository.GetEmployeesWithPaymentDataForMonth
                (request.CompanyId, request.Year, request.Month, request.ResourceParameters);

            var listOfEmloyeesWithWorkDaysAndPaymentInfoForMonth
                 = _mapper.Map<IEnumerable<Domain.Entities.Employee>>(employeesWithWorkDaysAndPaymentInfoForMonth);

            List<WageResponse> employeeWages = new List<WageResponse>();

            foreach (var employee in listOfEmloyeesWithWorkDaysAndPaymentInfoForMonth)
            {
                var wageResponse = _wageService.CalculateWageForMonth
                    (employee.WorkDays, employee.PaymentInfos, request.Month, request.Year, employee.Id);

                if (wageResponse != null)
                {
                    employeeWages.Add(wageResponse);
                }
            }

            response.Value = new PagedList<WageResponse>
                (employeeWages, request.ResourceParameters.PageNumber, 
                request.ResourceParameters.PageSize, employeesWithWorkDaysAndPaymentInfoForMonth.TotalCount);

            return response;
        }
    }
}
