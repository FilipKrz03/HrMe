using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.MonthlyBonus.Command
{
    public class MonthlyBonusRequest
    {
        public int Year {  get; set; }

        public int Month {  get; set; } 

        public double BonusAmount { get; set; } = 0;

        public double BonusInPercent {  get; set; } = 0;
    }
}
