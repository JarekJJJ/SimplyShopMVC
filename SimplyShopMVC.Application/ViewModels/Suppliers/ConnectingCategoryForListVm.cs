using SimplyShopMVC.Application.ViewModels.Item;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Application.ViewModels.Suppliers
{
    public class ConnectingCategoryForListVm
    {
        public CategoryForListVm category { get; set; }
        public List<IncomGroupForListVm> listIncomGroup { get; set; }
        public ItemTagsForListVm itemTagsForList { get; set; }
        public GroupItemForListVm groupItem { get; set; }
    }
}
