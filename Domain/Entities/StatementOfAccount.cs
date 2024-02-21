using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class StatementOfAccount
    {
        [Key]
        public int StatementOfAccountId { get; set; }
        public int? AccountCodeId { get; set; }
        public DateTime date { get; set; }
        public string DocNo { get; set; }
        public string Description { get; set; }  
        public int? ReportType { get; set; }
        public virtual AccountCode AccountCode { get; set; }
    }
}
