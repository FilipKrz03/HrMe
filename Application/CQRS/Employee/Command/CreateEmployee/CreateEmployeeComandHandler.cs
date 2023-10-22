using Application.CQRS.Employee.Response;
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

namespace Application.CQRS.Employee.Command.CreateEmployee
{
    public class CreateEmployeeComandHandler : IRequestHandler<CreateEmployeeCommand, Response<EmployeeResponse?>>
    {
        private readonly HrMeContext _context;
        private readonly IMapper _mapper;
        public CreateEmployeeComandHandler(HrMeContext context , IMapper mapper)
        {
            _context = context??throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        public async Task<Response<EmployeeResponse?>> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
        {
            Response<EmployeeResponse?> response = new();

            var employeeCompany = await _context.Companies
                .Where(c => c.Id == Guid.Parse(request.CompanyGuid))
                .Include(c => c.Employees)
                .FirstOrDefaultAsync(cancellationToken);

            if(employeeCompany == null)
            {
                response.SetError(500, "We occured some unexpected error");
                return response;
            }

            var employeeExist = await _context.Employees
                 .Where(e => e.Email == request.Email)
                 .FirstOrDefaultAsync(cancellationToken);

            if(employeeExist != null)
            {
                response.SetError(409,
                    $"The employe already exist in our database , and works in {employeeExist.Company.CompanyName} company");
                return response;
            }

            Domain.Entities.Employee employee
                = _mapper.Map<Domain.Entities.Employee>(request);

            employeeCompany.Employees.Add(employee);

            await _context.SaveChangesAsync(cancellationToken);

            response.Value = _mapper.Map<EmployeeResponse>(request);

            return response;
        }
    }
}
