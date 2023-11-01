using Domain.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.PaymentInfo.Command
{
    public class PaymentInfoRequest
    {
        public double HourlyRateBrutto { get; set; }

        public ContractType ContractType { get; set; }

        public DateTime StartOfContractDate { get; set; }

        public DateTime? EndOfContractDate { get; set; }
    }
}
