using Application.CQRS.WorkDay.Response;
using AutoMapper;
using Domain.Abstractions;
using Domain.Common.Responses;
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
        private readonly IComapniesContextRepostiory _companiesContextRepostiory;

        public CreateWorkDayCommandHandler
            (IMapper mapper, IComapniesContextRepostiory comapniesContexRepostiory)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _companiesContextRepostiory = comapniesContexRepostiory ??
                throw new ArgumentNullException(nameof(comapniesContexRepostiory));
        }

        public async Task<Response<WorkDayResponse>> Handle(CreateWorkDayCommand request, CancellationToken cancellationToken)
        {
            Response<WorkDayResponse> response = new();

            EmployeAndCompanyExist exist = await _companiesContextRepostiory
                .EmployeAndCompanyExistAsync(request.CompanyId, request.EmployeeId);

            if (!exist.CompanyExist)
            {
                return response.SetError(404, "We could not find your company");
            }

            if (!exist.EmployeeExist)
            {
                return response.SetError(404, $"We could not find employee with id {request.EmployeeId}");
            }

            var workDayExist = await _companiesContextRepostiory
                .EmployeeWorkDayExistAsync(request.WorkDayDate, request.EmployeeId);

            if (workDayExist)
            {
                return response.SetError
                      (409, $"Employee already has working day on {request.WorkDayDate.Date:MM/dd/yyyy}");
            }

            EmployeeWorkDay workDayEntity = _mapper.Map<EmployeeWorkDay>(request);

            _companiesContextRepostiory.CreateWorkDay(request.EmployeeId, workDayEntity);

            await _companiesContextRepostiory.SaveChangesAsync();

            response.Value = _mapper.Map<WorkDayResponse>(workDayEntity);

            return response;
        }
    }
}
