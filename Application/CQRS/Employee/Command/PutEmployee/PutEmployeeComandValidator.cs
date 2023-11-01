using Application.CQRS.Employee.Command.CreateEmployee;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Employee.Command.PutEmployee
{
    public class PutEmployeeComandValidator : AbstractValidator<PutEmployeeComand>
    {
        public PutEmployeeComandValidator()
        {
            RuleFor(p => p.EmployeeId).NotEmpty();
            RuleFor(p => p.CompanyId).NotEmpty();
            RuleFor(p => p.FirstName).NotEmpty().MaximumLength(128);
            RuleFor(p => p.LastName).NotEmpty().MaximumLength(128);
            RuleFor(p => p.Position).NotEmpty().MaximumLength(64);
            RuleFor(p => p.Email).NotEmpty().EmailAddress();
            RuleFor(p => p.DateOfBirth).Must(BeAValidDate);
        }
        private bool BeAValidDate(DateTime DateOfBirth)
        {
            if (DateOfBirth == default)
                return false;
            return true;
        }
    }
}
