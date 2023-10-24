using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Employee
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        public string Position { get; set; } = string.Empty;

        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public DateTime DateOfBirth { get; set; }

        public ICollection<EmployeeWorkDay> WorkDays { get; set; }
         = new List<EmployeeWorkDay>();

        public ICollection<EmployeePaymentInfo> PaymentInfos { get; set; }
            = new List<EmployeePaymentInfo>();

        [ForeignKey("CompanyId")]
        public Company Company { get; set; } = null!;
        public Guid CompanyId { get; set; } 
    }
}
