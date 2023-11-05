using Domain.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.MonthlyBonus.Query.GetMonthlyBonus
{
    public class GetMonthlyBonusCommand : IRequest<Response<MonthlyBonusResponse>>
    {
        public Guid MonthlyBonusId { get; set; }
        public Guid CompanyId { get; set; }

        public Guid EmployeeId { get; set; }

        public GetMonthlyBonusCommand(Guid monthlyBonusId ,Guid companyId, Guid employeeId)
        {
            MonthlyBonusId = monthlyBonusId;
            CompanyId = companyId;
            EmployeeId = employeeId;
        }
    }
}
