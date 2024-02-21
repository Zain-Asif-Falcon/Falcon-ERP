using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Domain.Entities
{
    public class PurchaseGood
    {
        public PurchaseGood()
        {
            PurchaseItems = new HashSet<PurchaseItem>();
        }
        [Key]
        public int PurchasesGoodsId { get; set; }
        public string DocumentNumber { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Party Code Required")]
        public int? AccountCodeId { get; set; }
        [Required]
        [Display(Name = "Date Purchase", Prompt = "Date Purchase Required")]
        public DateTime? DatePurchase { get; set; }
        public DateTime? DueDate { get; set; }
        [Required]
        [Range(1, Int32.MaxValue, ErrorMessage = "Amount Should be Greater than 0")]
        [Display(Prompt = "Products Selection Required")]
        public decimal? InvoiceAmount { get; set; }
        [Required]
        [Range(1, Int32.MaxValue, ErrorMessage = "Amount Should be Greater than 0")]
        public decimal? GrandTotal { get; set; }
        [Required(ErrorMessage = "Paying Amount Required")]
        [Range(0, Int32.MaxValue, ErrorMessage = "Amount Should be Greater than 0")]
        public decimal? PayingAmount { get; set; }
        public decimal? Labour { get; set; }
        public decimal? Loading { get; set; }
        public decimal? Cutting { get; set; }
      
        public DateTime? DateTimeEntered { get; set; }
        public int? DayOpeningsId { get; set; }
        public virtual DayOpenings DayOpening { get; set; }
        public virtual AccountCode AccountCode { get; set; }
        public virtual ICollection<PurchaseItem> PurchaseItems { get; set; }
    }
}
