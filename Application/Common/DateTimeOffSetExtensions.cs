using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common
{
    public static class DateTimeOffSetExtensions
    {
        public static int CalculateAge(DateTimeOffset birthDate)
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
    }
}
