using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common
{
    public static class TimeOnlyExtensions
    {
        public static int CalculateMinutesAfterMidnight(this TimeOnly time)
        {
            int minutes = 0;

            minutes = time.Hour * 60;
            minutes += time.Minute;

            return minutes;
        }

        public static TimeOnly CalculateTimeFromMinutesAfterMidnight(int minutes)
        {
            int allHours = minutes / 60;

            int allMinutes = minutes % 60;

            TimeOnly time = new TimeOnly(allHours , allMinutes);

            return time;
        }
    }
}
