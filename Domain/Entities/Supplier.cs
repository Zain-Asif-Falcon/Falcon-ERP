using Domain.AuditableEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Domain.Entities
{
    public partial class Supplier : AuditEntities
    {
        public Supplier()
        {
            PurchaseGoods = new HashSet<PurchaseGood>();
        }
        [Key]
        public int SuppliersId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        [Required]
        public string NtaxNumber { get; set; }
        public string Address { get; set; }
        public virtual ICollection<PurchaseGood> PurchaseGoods { get; set; }
    }
}
