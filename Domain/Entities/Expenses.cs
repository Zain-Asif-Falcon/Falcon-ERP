using Domain.AuditableEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
   public class Expenses : AuditEntities
    {
        public Expenses()
        {
        }
        [Key]
        public int ExpensesId { get; set; }
        [Required(ErrorMessage = "Amount Required")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$")]
        [Range(0, 9999999999999999.99)]
        public decimal? Amount { get; set; }
        [Required(ErrorMessage = "Date Selection Required")]       
        [Required(ErrorMessage = "Account Heads Required")]      
        public int? DayOpeningsId { get; set; }
        public virtual ExpenseHead ExpenseHeads { get; set; }
     
    }
}
