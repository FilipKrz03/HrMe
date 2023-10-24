using Application.CQRS.WorkDay.Response;
using AutoMapper;
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
        private readonly HrMeContext _context;

        public CreateWorkDayCommandHandler(HrMeContext context , IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));   
        }

        public async Task<Response<WorkDayResponse>> Handle(CreateWorkDayCommand request, CancellationToken cancellationToken)
        {
            Response<WorkDayResponse> response = new();

            var companyExist = await _context
                .Companies
                .AnyAsync(c => c.Id == request.CompanyId , cancellationToken);

            if(!companyExist)
            {
                response.SetError(404, "We could not find your company");
                return response;
            }

            var employee = await _context
                .Employees
                .Where(e => e.Id == request.EmployeeId && e.CompanyId == request.CompanyId)
                .FirstOrDefaultAsync(cancellationToken);

            if(employee == null)
            {
                response.SetError(404, $"We could not find employee with id {request.EmployeeId}");
                return response;
            }

            var workDayExist = await _context.EmployeesWorkDays
                .AnyAsync
                (w => w.WorkDayDate.Day == request.WorkDayDate.Day
                && w.WorkDayDate.Month == request.WorkDayDate.Month
                && w.WorkDayDate.Year == request.WorkDayDate.Year
                && request.EmployeeId == employee.Id , cancellationToken);    

            if (workDayExist)
            {
                response.SetError
                    (409, $"Employee already has working day on {request.WorkDayDate.Date:MM/dd/yyyy}");
                return response;
            }

            EmployeeWorkDay workDayEntity = _mapper.Map<EmployeeWorkDay>(request);

            employee.WorkDays.Add(workDayEntity);

            await _context.SaveChangesAsync(cancellationToken);

            response.Value = _mapper.Map<WorkDayResponse>(workDayEntity);

            return response;

        }
    }
}
