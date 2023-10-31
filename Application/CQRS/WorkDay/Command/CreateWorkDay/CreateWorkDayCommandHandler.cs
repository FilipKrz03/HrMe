using Domain.Responses;
using AutoMapper;
using Domain.Abstractions;
using Domain.Entities;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.WorkDay.Command.CreateWorkDay
{
    public class CreateWorkDayCommandHandler : IRequestHandler<CreateWorkDayCommand, Response<WorkDayResponse>>
    {

        private readonly IMapper _mapper;
        private readonly ICompanyRepository _companyRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IWorkDayReposiotry _workDayReposiotry;

        public CreateWorkDayCommandHandler
            (IMapper mapper , ICompanyRepository companyRepository ,
            IEmployeeRepository employeeRepostiory, IWorkDayReposiotry workDayReposiotry)
        {
            _mapper = mapper;
            _companyRepository = companyRepository;
            _employeeRepository = employeeRepostiory;
            _workDayReposiotry = workDayReposiotry; 
        }

        public async Task<Response<WorkDayResponse>> Handle(CreateWorkDayCommand request, CancellationToken cancellationToken)
        {
            Response<WorkDayResponse> response = new();

            if (! await _companyRepository.CompanyExistAsync(request.CompanyId))
            {
                return response.SetError(404, "We could not find your company");
            }

            if (! await _employeeRepository.EmployeeExistAsync(request.EmployeeId))
            {
                return response.SetError(404, $"We could not find employee with id {request.EmployeeId}");
            }

            var workDayExist = await _workDayReposiotry
                .WorkDayExistAsync(request.WorkDayDate, request.EmployeeId);

            if (workDayExist)
            {
                return response.SetError
                      (409, $"Employee already has working day on {request.WorkDayDate.Date:MM/dd/yyyy}");
            }

            EmployeeWorkDay workDayEntity = _mapper.Map<EmployeeWorkDay>(request);

            workDayEntity.EmployeeId = request.EmployeeId;

            await _workDayReposiotry.InsertWorkDay(workDayEntity);

            response.Value = _mapper.Map<WorkDayResponse>(workDayEntity);

            return response;
        }
    }
}
