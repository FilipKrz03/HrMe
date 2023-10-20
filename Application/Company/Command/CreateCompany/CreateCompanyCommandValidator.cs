using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.Company.Command.CreateCompany
{
    public class CreateCompanyCommandValidator : AbstractValidator<CreateCompanyCommand>
    {
        public CreateCompanyCommandValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.CompanyName).NotEmpty().MaximumLength(255);
            RuleFor(x => x.Password).NotEmpty().MinimumLength(5).MaximumLength(32);
        }
    }
}
