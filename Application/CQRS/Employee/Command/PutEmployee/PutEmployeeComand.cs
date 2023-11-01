using Domain.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Employee.Command.PutEmployee
{
    public class PutEmployeeComand : IRequest<Response<EmployeeResponse>>
    {
        public Guid CompanyId { get; set; }

        public Guid EmployeeId { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Position { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; }

        public PutEmployeeComand(Guid companyId, Guid employeeId ,  string firstName, string lastName,
            string position, string email, DateTime dateOffBirth)
        {
            CompanyId = companyId;
            EmployeeId = employeeId;
            FirstName = firstName;
            LastName = lastName;
            Position = position;
            Email = email;
            DateOfBirth = dateOffBirth;
        }
    }
}
