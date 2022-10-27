using Domain.Entities;
using Domain.ViewModel.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel.Transactions
{
    public class SalesOrderViewModel
    {
       public SalesOrderViewModel()
        {
            order = new SalesGoods();
            items = new List<SalesItems>();
        }
        public PartyCodes partyDet { get; set; }
        public SalesGoods order { get; set; }
        public List<SalesItems> items { get; set; }
    }
}
