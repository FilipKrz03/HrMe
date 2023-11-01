using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Employee.Command.DeleteEmployee
{
    public class DeleteEmployeeCommand : IRequest<Response<bool>>
    {
        public Guid CompanyId { get; set; }

        public Guid EmployeeId {  get; set; }

        public DeleteEmployeeCommand(Guid companyId , Guid employeeId)
        {
            CompanyId = companyId;
            EmployeeId = employeeId;
        }
    }
}
