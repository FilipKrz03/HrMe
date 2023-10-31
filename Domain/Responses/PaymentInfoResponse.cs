using Domain.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Responses
{
    public class PaymentInfoResponse
    {
        public Guid Id { get; set; }

        public double HourlyRateBrutto { get; set; }

        public ContractType ContractType { get; set; }

        public DateTime StartOfContractDate { get; set; }

        public DateTime? EndOfContractDate { get; set; }
    }
}
