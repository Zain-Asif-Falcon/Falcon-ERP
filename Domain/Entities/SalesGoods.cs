using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class SalesGoods
    {
        public SalesGoods()
        {
            SalesItems = new HashSet<SalesItems>();
        }
        [Key]
        public int SalesGoodsId { get; set; }
        public string DocumentNumber { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Party Code Required")]
        public int? AccountCodeId { get; set; }
        [Required]
        public DateTime? DateSales { get; set; }
        public DateTime? DueDate { get; set; }
        [Required]
        [Range(1, Int32.MaxValue, ErrorMessage = "Amount Should be Greater than 0")]
        [Display(Prompt = "Products Sum Value")]
        public decimal? InvoiceAmount { get; set; }
        [Required]
        [Range(1, Int32.MaxValue, ErrorMessage = "Amount Should be Greater than 0")]
        public decimal? GrandTotal { get; set; }
        [Required(ErrorMessage = "Recieving Amount Required")]
        [Range(0, Int32.MaxValue, ErrorMessage = "Amount Should be Greater than 0")]
        public decimal? PayingAmount { get; set; }
        public decimal? Labour { get; set; }
        public decimal? Loading { get; set; }
        public decimal? Cutting { get; set; }
        public decimal? Carriage { get; set; }
        public decimal? Carrogation { get; set; }
        public decimal? Misc { get; set; }
        public decimal? TotalExpenses { get; set; }
        public string ReferenceNum { get; set; }
        public string Notes { get; set; }
        public int? DayOpeningsId { get; set; }
        public bool? Acknowledged { get; set; }
        public DateTime? DateTimeEntered { get; set; }
        public virtual DayOpenings DayOpening { get; set; }
        public virtual AccountCode AccountCode { get; set; }
        public virtual ICollection<SalesItems> SalesItems { get; set; }
    }
}
