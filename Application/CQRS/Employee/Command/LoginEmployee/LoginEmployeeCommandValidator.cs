using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Employee.Command.LoginEmployee
{
    public class LoginEmployeeCommandValidator : AbstractValidator<LoginEmployeeCommand>
    {
        public LoginEmployeeCommandValidator()
        {
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}
