using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Company : BaseEntity
    {
     
        [Required]
        public string CompanyName { get; set; } = string.Empty;

        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]  
        public string Password { get; set; } = string.Empty;

        public ICollection<Employee> Employees { get; set; } =
            new List<Employee>();
    }
}
