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

namespace Application.CQRS.WorkDay.Command.PutWorkDay
{
    public class PutWorkDayCommandHandler : IRequestHandler<PutWorkDayCommand, Response<WorkDayResponse>>
    {

        private readonly IMapper _mapper;
        private readonly ICompanyRepository _companyRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IWorkDayReposiotry _workDayReposiotry;

        public PutWorkDayCommandHandler
            (IMapper mapper, ICompanyRepository companyRepository,
            IEmployeeRepository employeeRepostiory, IWorkDayReposiotry workDayReposiotry)
        {
            _mapper = mapper;
            _companyRepository = companyRepository;
            _employeeRepository = employeeRepostiory;
            _workDayReposiotry = workDayReposiotry;
        }
        public async Task<Response<WorkDayResponse>> Handle(PutWorkDayCommand request, CancellationToken cancellationToken)
        {
            Response<WorkDayResponse> response = new();

            if (!await _companyRepository.CompanyExistAsync(request.CompanyId))
            {
                return response.SetError(404, "We could not find your company");
            }

            if (!await _employeeRepository.EmployeeExistAsync(request.EmployeeId))
            {
                return response.SetError(404, $"We could not find employee with id {request.EmployeeId}");
            }

            var workDay = await _workDayReposiotry.GetWorkDayAsync(request.WorkDayId, request.EmployeeId);

            if (workDay == null)
            {
                var workDayExist = await _workDayReposiotry
                    .WorkDayExistAsync(request.WorkDayDate, request.EmployeeId);

                if (workDayExist)
                {
                    return response.SetError(409, $"Work day on date : {request.WorkDayDate} already exist");
                }

                EmployeeWorkDay workDayEntity = _mapper.Map<EmployeeWorkDay>(request);

                workDayEntity.EmployeeId = request.EmployeeId;
                workDayEntity.Id = request.WorkDayId;

                await _workDayReposiotry.InsertWorkDay(workDayEntity);

                response.Value = _mapper.Map<WorkDayResponse>(workDayEntity);

                return response;
            }

            var workDayExistWithSameDate = await
                _workDayReposiotry.OtherWorkDayExist(request.WorkDayDate , request.EmployeeId , request.WorkDayId);

            if(workDayExistWithSameDate)
            {
                return response.SetError(409, $"You want to change work day date " +
                    $"but this employee has already working day on {request.WorkDayDate}");
            }

            _mapper.Map(request, workDay);

            await _workDayReposiotry.SaveChangesAsync();

            return response;
        }
    }
}
