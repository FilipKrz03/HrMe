using Application.Common;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.WorkDay.Command.PutWorkDay
{
    public class PutWorkDayCommandValidator : AbstractValidator<PutWorkDayCommand>  
    {
        public PutWorkDayCommandValidator()
        {
            RuleFor(c => c.WorkDayId).NotEmpty();
            RuleFor(c => c.EmployeeId).NotEmpty();
            RuleFor(c => c.CompanyId).NotEmpty();
            RuleFor(t => new { t.StartTime, t.EndTime }).Must(x =>
           BeAValidEndTime(x.StartTime, x.EndTime))
               .WithMessage("End time must be at least 15 minut greater than start time");
        }

        private bool BeAValidEndTime(TimeOnly startTime, TimeOnly endTime)
        {
            int startTimeInMinutes =
                TimeOnlyExtensions.CalculateMinutesAfterMidnight(startTime);

            int endTimeInMinutes =
                TimeOnlyExtensions.CalculateMinutesAfterMidnight(endTime);

            return endTimeInMinutes - startTimeInMinutes >= 15;
        }
    }
}
