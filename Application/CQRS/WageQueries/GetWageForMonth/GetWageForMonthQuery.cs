using Domain.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.WageQueries.GetWageForMonth
{
    public class GetWageForMonthQuery : IRequest<Response<WageResponse>>
    {
        public Guid CompanyId { get; set; }

        public Guid EmployeeId {  get; set; }

        public int Year { get; set; }   

        public int Month { get; set; }

        public GetWageForMonthQuery(Guid companyId , Guid employeeId , int month , 
            int year)
        {
            CompanyId = companyId;
            EmployeeId = employeeId;
            Month = month;
            Year = year;
        }
    }
}
