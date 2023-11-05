using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.MonthlyBonus.Command.DeleteMonthlyBonus
{
    public class DeleteMonthlyBonusCommand : IRequest<Response<bool>>
    {
        public Guid CompanyId {  get; set; }    

        public Guid EmployeeId { get; set; }

        public Guid MonthlyBonusId { get; set; }

        public DeleteMonthlyBonusCommand(Guid companyId , Guid employeeId , Guid monthlyBonusId)
        {
            CompanyId = companyId;
            EmployeeId = employeeId;
            MonthlyBonusId = monthlyBonusId;
        }
    }
}
