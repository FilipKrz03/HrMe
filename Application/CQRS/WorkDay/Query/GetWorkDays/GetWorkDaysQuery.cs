using Application.CQRS.WorkDay.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.WorkDay.Query.GetWorkDays
{
    public class GetWorkDaysQuery : IRequest<Response<IEnumerable<WorkDayResponse>>>
    {
        public Guid CompanyId { get; set; }

        public Guid EmployeeId {  get; set; }

        public GetWorkDaysQuery(Guid companyId , Guid employeeId)
        {
            CompanyId = companyId;
            EmployeeId = employeeId;
        }
    }
}
