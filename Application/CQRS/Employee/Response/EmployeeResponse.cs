using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Employee.Response
{
   public class EmployeeResponse
    {
        public string FullName { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
    }
}
