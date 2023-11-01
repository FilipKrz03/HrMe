using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.WorkDay.Command.DeleteWorkDay
{
    public class DeleteWorkDayCommand : IRequest<Response<bool>>
    {
        public Guid CompanyId { get; set; }

        public Guid EmployeeId { get;set; }

        public Guid WorkDayId { get; set; }

        public DeleteWorkDayCommand(Guid companyId , Guid employeeId , 
            Guid workDayId)
        {
            CompanyId = companyId;
            EmployeeId = employeeId;    
            WorkDayId = workDayId;  
        }
    }
}
