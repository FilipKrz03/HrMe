﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class EmployeeWorkDay : BaseEntity
    {        
        public DateTime WorkDayDate { get; set; } 

        public int StartTimeInMinutesAfterMidnight {  get; set; }

        public int EndTimeInMinutesAfterMidnight { get; set; }

        [ForeignKey("EmployeeId")]
        public Employee Employee { get; set; } = null!;
        public Guid EmployeeId { get; set; }    

    }
}
