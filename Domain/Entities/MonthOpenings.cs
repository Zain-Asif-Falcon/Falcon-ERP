using Domain.AuditableEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class MonthOpenings : AuditEntities
    {
        [Key]
        public int MonthOpeningsId { get; set; }        
        public string Name { get; set; }
        [Required]
        public DateTime DateOpening { get; set; }       
        [RegularExpression("^[a-zA-Z ]*$")]
        public string Notes { get; set; }
    }
}
