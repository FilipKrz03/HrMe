using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common
{
    public static class DateTimeExtensions
    {
        public static int CalculateAge(DateTime birthDate)
        {
            int age = DateTime.Now.Year - birthDate.Year;

            if (birthDate.Month > DateTime.Now.Month)
            {
                age -= 1;
            }

            if ((birthDate.Month == DateTime.Now.Month) && birthDate.Day > DateTime.Now.Day)
            {
                age -= 1;
            }

            return age;
        }

        public static DateOnly ConvertDateTimeOffSetToDateOnly(DateTime date)
        {
            DateOnly dateOnly = new
                (date.Year, date.Month, date.Day);

            return dateOnly;
        }

        public static bool IsPaymentInfoDateAvaliable
            (DateTime start, DateTime? end, IEnumerable<Domain.Entities.EmployeePaymentInfo> paymentInfos)
        {
            foreach (Domain.Entities.EmployeePaymentInfo paymentInfo in paymentInfos)
            {

                if (start <= paymentInfo.EndOfContractDate && end >= paymentInfo.StartOfContractDate)
                {
                    return false;
                }

                if (paymentInfo.EndOfContractDate == null && start >= paymentInfo.StartOfContractDate)
                {
                    return false;
                }

                if (end == null && start <= paymentInfo.EndOfContractDate)
                {
                    return false;
                }

            }

            return true;
        }
    }
}
