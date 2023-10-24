using Application.CQRS.Employee.Response;
using AutoMapper;
using Azure;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Employee.Query.GetEmployees
{
    public class GetEmployeesQueryHandler : IRequestHandler<GetEmployeesQuery, Response<IEnumerable<EmployeeResponse>>>
    {
        private readonly HrMeContext _context;
        private readonly IMapper _mapper;

        public GetEmployeesQueryHandler(HrMeContext context, IMapper mapper)
        {
            _context = context ??
                throw new ArgumentNullException(nameof(context));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Response<IEnumerable<EmployeeResponse>>> 
            Handle(GetEmployeesQuery request, CancellationToken cancellationToken)
        {
            Response<IEnumerable<EmployeeResponse>> response = new();

            var comapnyExist = await _context.Companies
              .AnyAsync(e => e.Id == request.CompanyId, cancellationToken);

            if (!comapnyExist)
            {
                response.SetError(404, "We could not found your company in database");
                return response;
            }

            var employeList = await _context.Employees
                .Where(e => e.CompanyId == request.CompanyId)
                .ToListAsync();

            response.Value = _mapper.Map<IEnumerable<EmployeeResponse>>(employeList);

            return response;
        }
    }
}
