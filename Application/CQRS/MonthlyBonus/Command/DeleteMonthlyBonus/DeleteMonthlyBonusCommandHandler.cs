using AutoMapper;
using Domain.Abstractions;
using Domain.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.MonthlyBonus.Command.DeleteMonthlyBonus
{
    public class DeleteMonthlyBonusCommandHandler : IRequestHandler<DeleteMonthlyBonusCommand, Response<bool>>
    {
        
        private readonly ICompanyRepository _companyRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMonthlyBonusRepository _monthlyBonusRepository;

        public DeleteMonthlyBonusCommandHandler(ICompanyRepository companyRepository,
            IEmployeeRepository employeeRepository, IMonthlyBonusRepository monthlyBonusRepository)
        {
            _companyRepository = companyRepository;
            _employeeRepository = employeeRepository;
            _monthlyBonusRepository = monthlyBonusRepository;
        }
        public async Task<Response<bool>> Handle(DeleteMonthlyBonusCommand request, CancellationToken cancellationToken)
        {
            Response<bool> response = new();

            if (!await _companyRepository.CompanyExistAsync(request.CompanyId))
            {
                return response.SetError(404, "We could not find your company");
            }

            if (!await _employeeRepository.EmployeeExistAsync(request.EmployeeId))
            {
                return response.SetError(404, $"We could not find employee with id {request.EmployeeId}");
            }

            var monthlyBonus = await _monthlyBonusRepository.GetMonthlyBonus(request.EmployeeId, request.MonthlyBonusId);

            if(monthlyBonus == null)
            {
                return response.SetError(404, "We could not find monthly bonus");
            }

            await _monthlyBonusRepository.DeleteMonthlyBonusAsync(monthlyBonus);

            response.Value = true;
            return response;
        }
    }
}
