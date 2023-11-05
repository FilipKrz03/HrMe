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

namespace Application.CQRS.MonthlyBonus.Query.GetMonthlyBonuses
{
    public class GetMonthlyBonusesQueryHadnler : IRequestHandler<GetMonthlyBonusesQuery, Response<PagedList<MonthlyBonusResponse>>>
    {

        private readonly IMapper _mapper;
        private readonly ICompanyRepository _companyRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMonthlyBonusRepository _monthlyBonusRepostiory;

        public GetMonthlyBonusesQueryHadnler(IMapper mapper, IEmployeeRepository employeeRepository,
            ICompanyRepository companyRepository, IMonthlyBonusRepository monthlyBonusRepository)
        {
            _mapper = mapper;
            _employeeRepository = employeeRepository;
            _companyRepository = companyRepository;
            _monthlyBonusRepostiory = monthlyBonusRepository;
        }
        public async Task<Response<PagedList<MonthlyBonusResponse>>> Handle(GetMonthlyBonusesQuery request, CancellationToken cancellationToken)
        {
            Response<PagedList<MonthlyBonusResponse>> response = new();

            if (!await _companyRepository.CompanyExistAsync(request.CompanyId))
            {
                return response.SetError(404, "We could not find company");
            }

            if (!await _employeeRepository.EmployeeExistsInCompanyAsync(request.EmployeeId, request.CompanyId))
            {
                return response.SetError(404, "We could not find employee");
            }

            var monthlyBonusResponses =
                await _monthlyBonusRepostiory.GetEmployeeMonthlyBonuses(request.EmployeeId,
                request.ResourceParameters);

            response.Value = _mapper.Map<PagedList<MonthlyBonusResponse>>(monthlyBonusResponses);

            return response;
        }
    }
}
