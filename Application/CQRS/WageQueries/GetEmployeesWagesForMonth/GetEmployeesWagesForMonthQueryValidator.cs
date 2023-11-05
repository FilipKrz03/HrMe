using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.WageQueries.GetEmployeesWagesForMonth
{
    public class GetEmployeesWagesForMonthQueryValidator : AbstractValidator<GetEmployeesWagesForMonthQuery>
    {
        public GetEmployeesWagesForMonthQueryValidator()
        {
            RuleFor(x => x.Year).GreaterThan(2000).LessThan(2050).WithMessage("Enter valid year");
            RuleFor(x => x.Month).GreaterThan(0).LessThan(13).WithMessage("Enter valid month");
        }
    }
}
