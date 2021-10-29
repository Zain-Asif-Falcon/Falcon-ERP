using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel
{
    public class ItemsViewModel
    {
        public ItemsViewModel()
        {
            Itms = new Item();
        }
        public int stock { get; set; }
        public Item Itms { get; set; }
    }
}
