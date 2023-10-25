using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common.Responses
{
    public class EmployeAndCompanyExist
    {
        public bool CompanyExist { get; set; } = true;

        public bool EmployeeExist { get; set; } = true;  

    }
}
