using Application.CQRS.WorkDay.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.WorkDay.Command.CreateWorkDay
{
    public class CreateWorkDayCommand : IRequest<Response<WorkDayResponse?>>
    {
        public Guid CompanyId { get; set; }

        public Guid EmployeeId { get; set; }

        public DateTime WorkDayDate { get; set; }

        public TimeOnly StartTime { get; set; }

        public TimeOnly EndTime { get; set; }

        public CreateWorkDayCommand
            (Guid companyId,  Guid employeeId , DateTime workDayDate , TimeOnly startTime , TimeOnly endTime)
        {
            CompanyId = companyId;
            EmployeeId = employeeId;
            WorkDayDate = workDayDate;
            StartTime = startTime;
            EndTime = endTime;
        }
    }
}
