using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class EmployeePaymentInfo
    {
        public Guid Id { get; set; }

        public double HourlyRateBrutto { get; set; }

        public ContractType ContractType { get; set; }

        public DateTime StartOfContractDate { get; set; }

        public DateTime? EndOfContractDate { get; set; }

        [ForeignKey("EmployeeId")]
        public Employee Employee { get; set; } = null!;
        public Guid EmployeeId { get; set; }
    }
}
