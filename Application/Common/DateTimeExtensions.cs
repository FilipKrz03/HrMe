using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common
{
    public static class DateTimeExtensions
    {
        public static int CalculateAge(this DateTime birthDate)
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

        public static DateOnly ConvertDateTimeToDateOnly(this DateTime date)
        {
            DateOnly dateOnly = new
                (date.Year, date.Month, date.Day);

            return dateOnly;
        }
    }
}
