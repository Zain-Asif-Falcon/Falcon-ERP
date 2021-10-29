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
    public class AccountHead : AuditEntities
    {
        [Key]
        public int AccountHeadId { get; set; }
        [Column(TypeName = "NVARCHAR")]
        [StringLength(50)]
        [Required]
        public string Code { get; set; }
        [Column(TypeName = "NVARCHAR")]
        [StringLength(120)]
        [Required]
        public string Description { get; set; }
    }
}
