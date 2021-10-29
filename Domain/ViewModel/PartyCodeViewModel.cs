using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel
{
    public class PartyCodeViewModel
    {
        public PartyCodeViewModel()
        {
            partCode = new PartyCode();
        }
        public PartyCode partCode { get; set; }
    }
}
