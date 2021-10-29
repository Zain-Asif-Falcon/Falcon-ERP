using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel
{
    public class AccountHeadsViewModel
    {
        public AccountHeadsViewModel()
        {
            AccHead = new AccountHead();
        }
        public AccountHead AccHead { get; set; }
    }
}
