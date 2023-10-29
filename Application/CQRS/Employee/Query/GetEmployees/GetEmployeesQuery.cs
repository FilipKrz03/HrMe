using Application.CQRS.Employee.Response;
using Domain.Common;
using Infrastructure.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Employee.Query.GetEmployees
{
    public class GetEmployeesQuery : IRequest<Response<PagedList<EmployeeResponse>>>
    {
        public Guid CompanyId;

        public ResourceParameters ResourceParameters;

        public GetEmployeesQuery(Guid companyId, ResourceParameters resourceParameters)
        {
            CompanyId = companyId;
            ResourceParameters = resourceParameters;
        }
    }
}
