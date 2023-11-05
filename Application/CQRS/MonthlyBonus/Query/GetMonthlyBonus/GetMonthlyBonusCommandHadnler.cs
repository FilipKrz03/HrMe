using AutoMapper;
using Azure;
using Domain.Abstractions;
using Domain.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.MonthlyBonus.Query.GetMonthlyBonus
{
    public class GetMonthlyBonusCommandHadnler : IRequestHandler<GetMonthlyBonusCommand, Response<MonthlyBonusResponse>>
    {

        private readonly IMapper _mapper;
        private readonly ICompanyRepository _companyRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMonthlyBonusRepository _monthlyBonusRepository;

        public GetMonthlyBonusCommandHadnler(IMapper mapper, IEmployeeRepository employeeRepository,
            ICompanyRepository companyRepository, IMonthlyBonusRepository monthlyBonusRepository)
        {
            _mapper = mapper;
            _employeeRepository = employeeRepository;
            _companyRepository = companyRepository;
            _monthlyBonusRepository = monthlyBonusRepository;
        }
        public async Task<Response<MonthlyBonusResponse>> Handle(GetMonthlyBonusCommand request, CancellationToken cancellationToken)
        {
            Response<MonthlyBonusResponse> response = new();

            if (!await _companyRepository.CompanyExistAsync(request.CompanyId))
            {
                return response.SetError(404, "We could not find company");
            }

            if (!await _employeeRepository.EmployeeExistsInCompanyAsync(request.EmployeeId, request.CompanyId))
            {
                return response.SetError(404, "We could not find employee");
            }

            var monthlyBonus = await _monthlyBonusRepository
                .GetMonthlyBonus(request.EmployeeId, request.MonthlyBonusId);

            if (monthlyBonus == null)
            {
                return response.SetError(404, "Could not find monthly bonus");
            }

            response.Value = _mapper.Map<MonthlyBonusResponse>(monthlyBonus);

            return response;
        }
    }
}
