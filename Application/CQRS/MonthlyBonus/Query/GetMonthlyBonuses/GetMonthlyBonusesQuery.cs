using Domain.Common;
using Domain.Responses;
using Infrastructure.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.MonthlyBonus.Query.GetMonthlyBonuses
{
    public class GetMonthlyBonusesQuery : IRequest<Response<PagedList<MonthlyBonusResponse>>>
    {
        public Guid CompanyId { get; set; }

        public Guid EmployeeId { get; set; }

        public ResourceParameters ResourceParameters { get; set; }

        public GetMonthlyBonusesQuery(Guid companyId , Guid employeeId , 
            ResourceParameters resourceParamethers)
        {
            CompanyId = companyId;
            EmployeeId = employeeId;
            ResourceParameters = resourceParamethers;
        }
    }
}
