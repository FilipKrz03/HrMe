using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Company.Command.LoginCompany
{
    public class LoginComapnyCommandValidator : AbstractValidator<LoginCompanyCommand>
    {
        public LoginComapnyCommandValidator()
        {
            RuleFor(p => p.Email).NotEmpty().EmailAddress();
            RuleFor(p => p.Password).MinimumLength(5).MaximumLength(32);
        }
    }
}
