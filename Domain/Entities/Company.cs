using Domain.AuditableEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Company : AuditEntities
    {
        public Company()
        {
        }
        [Key]
        public int CompanyId { get; set; }
        [Required(ErrorMessage = "Name Required")]
        public string Name { get; set; }
        public string Address { get; set; }
        public string phone { get; set; }
        [Required]
        public string NTN { get; set; }
        public string STRN { get; set; }
    }
}
