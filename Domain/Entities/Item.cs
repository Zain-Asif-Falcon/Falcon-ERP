using Domain.AuditableEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Domain.Entities
{
    public class Item : AuditEntities
    {
        public Item()
        {
            PurchaseItems = new HashSet<PurchaseItem>();
            SalesItem = new HashSet<SalesItems>();
        }
        [Key]
        public int ItemsId { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public string Description { get; set; }
        //[Required]
        //[Range(1, 999999999.99)]
        public int? Weight { get; set; }
        //[Required]
        //[Range(1, 999999999.99)]
        public decimal? Price { get; set; }
        //[Required]
        //[Range(1, 999999999.99)]
        public decimal? PurchasePrice { get; set; }
        public ICollection<ItemStock> ItemStocks { get; set; }
        public virtual ICollection<PurchaseItem> PurchaseItems { get; set; }
        public virtual ICollection<SalesItems> SalesItem { get; set; }
    }
}
