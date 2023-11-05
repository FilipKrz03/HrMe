using Domain.Abstractions;
using Domain.Entities;
using Domain.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class WageService : IWageService
    {
        public WageResponse? CalculateWageForMonth
            (IEnumerable<EmployeeWorkDay> workDays,
            IEnumerable<EmployeePaymentInfo> paymentInfos,
            int month , int year , Guid employeeId , EmployeeMonthlyBonus? bonus)
        {
            double totalWageBrutto = 0;
            double totalWageNetto = 0;
            int totalMinutesWorked = 0;

            foreach (EmployeeWorkDay workDay in workDays)
            {
                var validPaymentInfo =
                    GetValidPaymentInfoForWorkDay(workDay, paymentInfos);

                if (validPaymentInfo == null)
                {
                    return null;
                }

                int minutesWorked =
                    workDay.EndTimeInMinutesAfterMidnight - workDay.StartTimeInMinutesAfterMidnight;

                totalMinutesWorked += minutesWorked;

                double wageBrutto = (minutesWorked / 60) * validPaymentInfo.HourlyRateBrutto;

                if(bonus != null)
                {
                    wageBrutto *= (1 + bonus.BonusInPercent/100);
                }

                totalWageBrutto += wageBrutto;

                totalWageNetto += validPaymentInfo.ContractType is
                    (Domain.Common.Enums.ContractType)1 or (Domain.Common.Enums.ContractType)2 ?
                    wageBrutto * 0.77 : wageBrutto;
            }

            if(bonus != null)
            {
                totalWageBrutto += bonus.BonusAmount;
                totalWageNetto += bonus.BonusAmount;
            }

            double totalHoursWorked = totalMinutesWorked / 60;

            return new WageResponse
                (totalWageNetto, totalWageBrutto, totalHoursWorked, month , year , employeeId);
        }

        private EmployeePaymentInfo? GetValidPaymentInfoForWorkDay(
            EmployeeWorkDay workDay, IEnumerable<EmployeePaymentInfo> paymentInfos)
        {
            foreach (EmployeePaymentInfo paymentInfo in paymentInfos)
            {
                if ((paymentInfo.StartOfContractDate.Date<= workDay.WorkDayDate.Date)
                  && (paymentInfo.EndOfContractDate == null ||
                  paymentInfo.EndOfContractDate.Value.Date >= workDay.WorkDayDate.Date))
                {
                    return paymentInfo;
                }
            }
            return null;
        }
    }
}
