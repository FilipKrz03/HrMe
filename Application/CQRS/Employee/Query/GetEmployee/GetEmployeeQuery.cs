using Application.CQRS.Employee.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Employee.Query.GetEmployee
{
    public class GetEmployeeQuery : IRequest<Response<EmployeeResponse>>
    {
        public Guid EmployeeId { get; set; }

        public Guid CompanyId { get; set; }

        public GetEmployeeQuery(Guid employeeId , Guid companyId)
        {
            EmployeeId = employeeId;
            CompanyId = companyId;
        }
    }
}
