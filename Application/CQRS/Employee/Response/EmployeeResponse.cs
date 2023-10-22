using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Employee.Response
{
   public class EmployeeResponse
    {
        public Guid Id { get; set; }    
        public string FullName { get; set; } = string.Empty;

        public string Position { get; set; } = string.Empty;

        public int Age { get; set; }
    }
}
