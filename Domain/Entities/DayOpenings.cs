using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class DayOpenings
    {
        [Key]
        public int DayOpeningsId { get; set; }
        public int? MonthOpeningsId { get; set; }
        //[Required]
        public DateTime DateTimeOpening { get; set; }
     
        public decimal? cashDifference { get; set; }
        public decimal? ClosingBalance { get; set; }
        public bool? closeStatus { get; set; }
        //[RegularExpression("^[a-zA-Z ]*$")]
        public string Notes { get; set; }
        public virtual MonthOpenings MonthOpening { get; set; }
    }
}
