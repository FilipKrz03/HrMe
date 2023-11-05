using Domain.Entities;
using Domain.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Abstractions
{
    public interface IWageService
    {
        WageResponse? CalculateWageForMonth
          (IEnumerable<EmployeeWorkDay> workDays,
             IEnumerable<EmployeePaymentInfo> paymentInfos,
             int month , int year , Guid employeeId , EmployeeMonthlyBonus bonus);


    }
}
