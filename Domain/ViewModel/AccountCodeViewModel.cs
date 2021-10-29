using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel
{
    public class AccountCodeViewModel
    {
        public AccountCodeViewModel()
        {
            AccCode = new AccountCode();
        }
        public string AccHead { get; set; }
        public AccountCode AccCode { get; set; }
    }
}
