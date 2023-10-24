using Application.CQRS.Employee.Response;
using AutoMapper;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Employee.Query.GetEmployee
{
    public class GetEmployeeQueryHandler : IRequestHandler<GetEmployeeQuery, Response<EmployeeResponse>>
    {
        private readonly HrMeContext _context;
        private readonly IMapper _mapper;

        public GetEmployeeQueryHandler(HrMeContext context , IMapper mapper)
        {
            _context = context ?? 
                throw new ArgumentNullException(nameof(context));   
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));    
        }
        public async Task<Response<EmployeeResponse>> Handle(GetEmployeeQuery request, CancellationToken cancellationToken)
        {
            Response<EmployeeResponse> response = new();

            var comapnyExist = await _context.Companies
                .AnyAsync(e => e.Id == request.CompanyId , cancellationToken);

            if(!comapnyExist)
            {
                response.SetError(404, "We could not found your company in database");
                return response;
            }

            var employee = await _context.Employees
                .Where(e => e.Id == request.EmployeeId && e.CompanyId == request.CompanyId)
                .FirstOrDefaultAsync(cancellationToken);

            if(employee == null)
            {
                response.SetError(404, $"We could not find employee with id {request.EmployeeId}");
                return response;
            }

            response.Value = _mapper.Map<EmployeeResponse>(employee);

            return response;
        }
    }
}
