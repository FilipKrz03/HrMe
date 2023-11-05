using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Responses
{
    public class WageResponse
    {
        public Guid EmployeeId {  get; set; }   

        public double WageNetto { get; set; }

        public double WageBrutto { get; set; }

        public double HoursWorked { get; set; }

        public int Month { get; set; }

        public int Year { get; set; }   

        public WageResponse(double wageNetto, double wageBrutto,
            double hoursWorked, int month, int year, Guid employeeId)
        {
            WageNetto = wageNetto;
            WageBrutto = wageBrutto;
            HoursWorked = hoursWorked;
            Month = month;
            Year = year;
            EmployeeId = employeeId;    
        }

    }
}
