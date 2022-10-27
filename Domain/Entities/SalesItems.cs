using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
   public class SalesItems
    {
        [Key]
        public int SalesItemsId { get; set; }
        public int? SalesGoodsOrderId { get; set; }
        [Required]
        [Display(Name = "Item", Prompt = "Item Required")]
        [Range(1, int.MaxValue, ErrorMessage = "Item Required")]
        public int? ItemsId { get; set; }
        [Required]
        [Range(1, Int32.MaxValue, ErrorMessage = "Quantity should be greater than or equal to 1")]
        [Display(Name = "Quantity", Prompt = "Qty Required")]
        public int? Qty { get; set; }
        public decimal? SalePrice { get; set; }
        public string Notes { get; set; }
        public DateTime? DateTimeEntered { get; set; }
        public virtual Item Items { get; set; }
        public virtual SalesGoods SalesGoodsOrder { get; set; }
    }
}
