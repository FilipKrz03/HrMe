using Domain.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.MonthlyBonus.Command.PutMonthlyBonus
{
    public class PutMonthlyBonusCommand : IRequest<Response<MonthlyBonusResponse>>
    {
        public Guid CompanyId { get; set; }

        public Guid EmployeeId {  get; set; }   

        public Guid MonthlyBonusId {  get; set; }

        public int Year { get; set; }

        public int Month { get; set; }

        public double BonusAmount { get; set; }

        public double BonusInPercent { get; set; }

        public PutMonthlyBonusCommand(Guid companyId, Guid employeeId, Guid monthlyBonusId ,  int year, int month,
            double bonusAmount, double bonusInPercent)
        {
            CompanyId = companyId;
            EmployeeId = employeeId;
            MonthlyBonusId = monthlyBonusId;
            Year = year;
            Month = month;
            BonusAmount = bonusAmount;
            BonusInPercent = bonusInPercent;
        }
    }
}
