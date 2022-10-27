using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel.Transactions
{
    public class SaleNoteListVM
    {
        public int SalesGoodsId { get; set; }
        public string DocumentNumber { get; set; }        
        public string CustomerName { get; set; }        
        public DateTime? DateSales { get; set; }
        public DateTime? DueDate { get; set; }        
        public decimal? InvoiceAmount { get; set; }        
        public decimal? GrandTotal { get; set; }        
        public decimal? PayingAmount { get; set; }
        public decimal? TotalExpenses { get; set; }
    }
}
