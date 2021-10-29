using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel
{
    public class CompanyViewModel
    {
        public CompanyViewModel()
        {
            comp = new Company();
        }
        public Company comp { get; set; }
    }
}
