using Application.CQRS.Employee.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Employee.Command.CreateEmployee
{
    public class CreateEmployeeCommand : IRequest<Response<EmployeeResponse?>>
    {
        public string CompanyGuid { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Position { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;
    }
}
