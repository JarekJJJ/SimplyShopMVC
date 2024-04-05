using SimplyShopMVC.Application.ViewModels.Item;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Application.ViewModels.Suppliers
{
    public class ListConnectingCategoryVm
    {
        public List<IncomGroupForListVm> listIncomGroups { get; set; } = new List<IncomGroupForListVm>();
        public List<CategoryForListVm> listCategories { get; set; } = new List<CategoryForListVm>();
        public List<ConnectingCategoryForListVm> listConnectionCategory { get; set; } = new List<ConnectingCategoryForListVm>();
        public List<ItemTagsForListVm> listItemTags { get; set; } = new List<ItemTagsForListVm>();
        public List<GroupItemForListVm> listGroupItems { get; set; } = new List<GroupItemForListVm>();
        public IncomGroupForListVm incomGroup { get; set; } = new IncomGroupForListVm();
        public int options { get; set; }
    }
}
