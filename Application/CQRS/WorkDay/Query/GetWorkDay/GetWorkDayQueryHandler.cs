using Application.CQRS.WorkDay.Response;
using AutoMapper;
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
        private readonly HrMeContext _context;

        public GetWorkDayQueryHandler(HrMeContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Response<WorkDayResponse>> Handle(GetWorkDayQuery request, CancellationToken cancellationToken)
        {
            Response<WorkDayResponse> response = new();

            var companyExist = await _context
              .Companies
              .AnyAsync(c => c.Id == request.CompanyId, cancellationToken);

            if (!companyExist)
            {
                response.SetError(404, "We could not find your company");
                return response;
            }

            var employeeExist = await _context
                .Employees
                .AnyAsync(e => e.Id == request.EmployeeId && e.CompanyId == request.CompanyId, cancellationToken);
          

            if (employeeExist == false)
            {
                response.SetError(404, $"We could not find employee with id {request.EmployeeId}");
                return response;
            }

            var workDay = await _context.EmployeesWorkDays
                .Where(w => w.Id == request.WorkDayId && w.EmployeeId == request.EmployeeId)
                .FirstOrDefaultAsync(cancellationToken);

            if (workDay == null)
            {
                response.SetError(404 , $"We could not find work day with id {request.WorkDayId}");
            }

            response.Value = _mapper.Map<WorkDayResponse>(workDay);

            return response;
        }
    }
}
