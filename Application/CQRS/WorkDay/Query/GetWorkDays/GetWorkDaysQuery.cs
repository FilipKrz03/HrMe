using Application.CQRS.WorkDay.Response;
using Domain.Common;
using Infrastructure.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.WorkDay.Query.GetWorkDays
{
    public class GetWorkDaysQuery : IRequest<Response<PagedList<WorkDayResponse>>>
    {
        public Guid CompanyId { get; set; }

        public Guid EmployeeId {  get; set; }

        public ResourceParameters ResourceParameters { get; set; }

        public GetWorkDaysQuery
            (Guid companyId , Guid employeeId , ResourceParameters resourceParameters)
        {
            CompanyId = companyId;
            EmployeeId = employeeId;
            ResourceParameters = resourceParameters;
        }
    }
}
