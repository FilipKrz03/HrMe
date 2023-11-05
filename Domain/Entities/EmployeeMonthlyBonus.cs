using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class EmployeeMonthlyBonus : BaseEntity
    {
        [Required]
        public int Year { get; set; }

        [Required]
        public int Month { get; set; }

        public double BonusAmount { get; set; }

        public double BonusInPercent { get; set; }

        [ForeignKey("EmployeeId")]
        public Employee Employee { get; set; } = null!;
        public Guid EmployeeId { get; set; }
    }
}
