using AutoMapper;
using Azure;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.MonthlyBonus.Command.CreateMonthlyBonus
{
    public class CreateMonthlyBonusCommandHandler : IRequestHandler<CreateMonthlyBonusCommand, Response<MonthlyBonusResponse>>
    {

        private readonly IMapper _mapper;
        private readonly ICompanyRepository _companyRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMonthlyBonusRepository _monthlyBonusRepository;


        public CreateMonthlyBonusCommandHandler(IMapper mapper, ICompanyRepository companyRepository,
            IEmployeeRepository employeeRepository , IMonthlyBonusRepository monthlyBonusRepository)
        {
            _mapper = mapper;
            _companyRepository = companyRepository;
            _employeeRepository = employeeRepository;
            _monthlyBonusRepository = monthlyBonusRepository;
        }

        public async Task<Response<MonthlyBonusResponse>> Handle(CreateMonthlyBonusCommand request, CancellationToken cancellationToken)
        {
            Response<MonthlyBonusResponse> response = new();

            if (!await _companyRepository.CompanyExistAsync(request.CompanyId))
            {
                return response.SetError(404, "We could not find your company");
            }

            if (!await _employeeRepository.EmployeeExistAsync(request.EmployeeId))
            {
                return response.SetError(404, $"We could not find employee with id {request.EmployeeId}");
            }

            var monthlyBonus = _mapper.Map<EmployeeMonthlyBonus>(request);
            monthlyBonus.EmployeeId = request.EmployeeId;

            await _monthlyBonusRepository.InsertMonthlyBonus(monthlyBonus);

            response.Value = _mapper.Map<MonthlyBonusResponse>(monthlyBonus);

            return response;
        }
    }
}
