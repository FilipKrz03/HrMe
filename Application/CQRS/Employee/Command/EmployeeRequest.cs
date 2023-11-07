using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Employee.Command
{
    public class EmployeeRequest
    {
        public string FirstName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Position { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; }

        public string? Password {  get; set; }  
    }
}
