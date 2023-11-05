using Domain.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.MonthlyBonus.Command.CreateMonthlyBonus
{
    public class CreateMonthlyBonusCommand : IRequest<Response<MonthlyBonusResponse>>
    {
        public Guid CompanyId {  get; set; }    

        public Guid EmployeeId { get; set; }

        public int Year { get;set; }

        public int Month { get; set; }

        public double BonusAmount {  get; set; }   

        public double BonusInPercent {  get; set; }

        public CreateMonthlyBonusCommand(Guid companyId , Guid employeeId, int year , int month,
            double bonusAmount, double bonusInPercent)
        {
            CompanyId = companyId;
            EmployeeId = employeeId;
            Year = year;
            Month = month;
            BonusAmount = bonusAmount;
            BonusInPercent = bonusInPercent;
        }
    }
}
