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

namespace Application.CQRS.MonthlyBonus.Command.PutMonthlyBonus
{
    public class PutMonthlyBonusCommandHandler : IRequestHandler<PutMonthlyBonusCommand, Response<MonthlyBonusResponse>>
    {

        private readonly IMapper _mapper;
        private readonly ICompanyRepository _companyRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMonthlyBonusRepository _monthlyBonusRepository;

        public PutMonthlyBonusCommandHandler(IMapper mapper, ICompanyRepository companyRepository,
            IEmployeeRepository employeeRepository, IMonthlyBonusRepository monthlyBonusRepository)
        {
            _mapper = mapper;
            _companyRepository = companyRepository;
            _employeeRepository = employeeRepository;
            _monthlyBonusRepository = monthlyBonusRepository;
        }
        public async Task<Response<MonthlyBonusResponse>> Handle(PutMonthlyBonusCommand request, CancellationToken cancellationToken)
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

            var monthlyBonus = await 
                _monthlyBonusRepository.GetMonthlyBonus(request.EmployeeId , request.MonthlyBonusId);

            if(monthlyBonus == null)
            {
               
                if(await _monthlyBonusRepository.MonthlyBonusDateNotAvaliable(request.EmployeeId , request.Year , request.Month))
                {
                    return response.SetError
                        (409, $"Monthly bonus already exist on {request.Year}/{request.Month}");
                }

                var monthlyBonusEntity = _mapper.Map<EmployeeMonthlyBonus>(request);
                monthlyBonusEntity.EmployeeId = request.EmployeeId;
                monthlyBonusEntity.Id = request.MonthlyBonusId;

                await _monthlyBonusRepository.InsertMonthlyBonus(monthlyBonusEntity);

                response.Value = _mapper.Map<MonthlyBonusResponse>(monthlyBonusEntity);

                return response;
            }

            if(await _monthlyBonusRepository
                .OtherMonthlyBonusDateExist(request.EmployeeId , request.MonthlyBonusId , request.Year , request.Month))
            {
                return response
                    .SetError(409, $"You are trying to change bonus data to date that already exist in other bonus");
            }

            _mapper.Map(request, monthlyBonus);

            await _monthlyBonusRepository.SaveChangesAsync();

            return response;
        }
    }
}
