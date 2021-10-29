using Domain.AuditableEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class AccountCode : AuditEntities
    {
        public AccountCode()
        {
        }
        [Key]
        public int AccountCodeId { get; set; }
        [Required(ErrorMessage ="Account Code Required")]
        public string Code { get; set; }
        public string Description { get; set; }
        [Required(ErrorMessage = "Opening Balance Required")]        
        public decimal? OpeningBalance { get; set; }
        [Required(ErrorMessage = "Account Head Required")]
        public int AccountHeadId { get; set; }
        public virtual AccountHead AccountHead { get; set; }
    }
}
