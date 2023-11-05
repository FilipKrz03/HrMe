using Domain.Common;
using Domain.Responses;
using Infrastructure.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.WageQueries.GetEmployeesWagesForMonth
{
    public class GetEmployeesWagesForMonthQuery : IRequest<Response<PagedList<WageResponse>>>
    {

        public Guid CompanyId {  get; set; }    

        public ResourceParameters ResourceParameters { get; set; }

        public int Year {  get; set; }  

        public int Month { get; set; }

        public GetEmployeesWagesForMonthQuery(ResourceParameters resourceParameters , Guid companyId ,
            int year , int month)
        {
            ResourceParameters = resourceParameters;
            CompanyId = companyId;
            Year = year;
            Month = month;
        }
    }
}
