using Application.CQRS.WorkDay.Response;
using AutoMapper;
using Domain.Abstractions;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.WorkDay.Query.GetWorkDay
{
    public class GetWorkDayQueryHandler : IRequestHandler<GetWorkDayQuery, Response<WorkDayResponse>>
    {

        private readonly IMapper _mapper;
        private readonly IWorkDayReposiotry _workDayRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public GetWorkDayQueryHandler
            (IMapper mapper,IWorkDayReposiotry workDayRepository
            , ICompanyRepository companyRepository, IEmployeeRepository employeeRepository)
        {
            _mapper = mapper;
            _workDayRepository = workDayRepository;
            _companyRepository = companyRepository;
            _employeeRepository = employeeRepository;   
        }

        public async Task<Response<WorkDayResponse>> Handle(GetWorkDayQuery request, CancellationToken cancellationToken)
        {
            Response<WorkDayResponse> response = new();

            if (!await _companyRepository.CompanyExistAsync(request.CompanyId))
            {
                return response.SetError(404, "We could not find your company");
            }

            if (! await _employeeRepository.EmployeeExistAsync(request.EmployeeId))
            {
                return
                    response.SetError(404, $"We could not find employee with id {request.EmployeeId}");
            }

            var workDay = await _workDayRepository
                .GetWorkDayAsync(request.WorkDayId , request.EmployeeId);

            if (workDay == null)
            {
                return
                    response.SetError(404, $"We could not find work day with id {request.WorkDayId}");
            }

            response.Value = _mapper.Map<WorkDayResponse>(workDay);

            return response;
        }
    }
}
