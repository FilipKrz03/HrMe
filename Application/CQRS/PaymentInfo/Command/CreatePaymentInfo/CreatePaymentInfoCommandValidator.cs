using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.PaymentInfo.Command.CreatePaymentInfo
{
    public class CreatePaymentInfoCommandValidator : AbstractValidator<CreatePaymentInfoCommand>
    {
        public CreatePaymentInfoCommandValidator()
        {
            RuleFor(src => src.HourlyRateBrutto).NotEmpty()
                .GreaterThanOrEqualTo(23.5)
                .WithMessage("Minimal wage in poland is 23.50");
            RuleFor(src => new { src.StartOfContractDate, src.EndOfContractDate })
                .Must(x => BeAValidContractTime(x.StartOfContractDate, x.EndOfContractDate))
                .WithMessage
                ("Contract end time must be greater than contract start time");
        }

        private bool BeAValidContractTime
            (DateTime contractStartTime, DateTime? contractEndTime)
        {
            if (contractEndTime == null) return true;

            int compareResult = 
                DateTime.Compare(contractStartTime, (DateTime)contractEndTime);

            return compareResult < 0;
        }
    }
}
