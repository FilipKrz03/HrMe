using Domain.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.WorkDay.Command.PutWorkDay
{
    public class PutWorkDayCommand : IRequest<Response<WorkDayResponse>>
    {
        public Guid CompanyId { get; set; }

        public Guid EmployeeId { get; set; }

        public Guid WorkDayId { get; set; }

        public DateTime WorkDayDate { get; set; }

        public TimeOnly StartTime { get; set; }

        public TimeOnly EndTime { get; set; }

        public PutWorkDayCommand
            (Guid companyId, Guid employeeId, Guid workDayId ,  
            DateTime workDayDate, TimeOnly startTime, TimeOnly endTime)
        {
            CompanyId = companyId;
            EmployeeId = employeeId;
            WorkDayId = workDayId;
            WorkDayDate = workDayDate;
            StartTime = startTime;
            EndTime = endTime;
        }
    }
}
