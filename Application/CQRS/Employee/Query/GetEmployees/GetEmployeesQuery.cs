using Application.CQRS.Employee.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Employee.Query.GetEmployees
{
    public class GetEmployeesQuery : IRequest<Response<IEnumerable<EmployeeResponse>>>
    {
        public Guid CompanyId;

        public GetEmployeesQuery(Guid companyId)
        {
            CompanyId = companyId;            
        }
    }
}
