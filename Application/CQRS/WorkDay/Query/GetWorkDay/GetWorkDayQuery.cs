using Domain.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.WorkDay.Query.GetWorkDay
{
    public class GetWorkDayQuery : IRequest<Response<WorkDayResponse>>
    {
        public Guid CompanyId { get; set; }

        public Guid EmployeeId { get; set; }

        public Guid WorkDayId { get; set; }

        public GetWorkDayQuery(Guid companyId , Guid employeeId  , Guid workDayId)
        {
            CompanyId = companyId;
            EmployeeId = employeeId;
            WorkDayId = workDayId;
        }

    }
}
