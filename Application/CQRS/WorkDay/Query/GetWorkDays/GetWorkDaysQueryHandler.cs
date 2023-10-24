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

namespace Application.CQRS.WorkDay.Query.GetWorkDays
{
    public class GetWorkDaysQueryHandler : IRequestHandler<GetWorkDaysQuery, Response<IEnumerable<WorkDayResponse>>>
    {
        private readonly IMapper _mapper;
        private readonly HrMeContext _context;

        public GetWorkDaysQueryHandler(HrMeContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Response<IEnumerable<WorkDayResponse>>> Handle(GetWorkDaysQuery request, CancellationToken cancellationToken)
        {

            Response<IEnumerable<WorkDayResponse>> response = new();

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

            var workDays = await _context.EmployeesWorkDays
                .Where(w => w.EmployeeId == request.EmployeeId)
                .ToListAsync(cancellationToken);

            response.Value = _mapper.Map<IEnumerable<WorkDayResponse>>(workDays);

            return response;
        }
    }
}
