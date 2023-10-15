using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Application.ViewModels.Item
{
    public  class ListItemForListVm
    {
        public List<ItemForListVm> Items { get; set; }
        public List<ItemTagsForListVm> Tags { get; set; }  
        public List<CategoryForListVm> Category { get; set; }
        public int Count { get; set; }
    }
}
