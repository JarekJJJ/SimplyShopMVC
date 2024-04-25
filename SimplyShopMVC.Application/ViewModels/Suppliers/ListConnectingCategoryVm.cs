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
        public IncomGroupForListVm incomGroup { get; set; }
        public List<CategoryForListVm> listCategories { get; set; } = new List<CategoryForListVm>();
        public List<ConnectingCategoryForListVm> listConnectionCategory { get; set; } = new List<ConnectingCategoryForListVm>();
        public ConnectingCategoryForListVm connectingCategory { get; set; }
        public List<ItemTagsForListVm> listItemTags { get; set; } = new List<ItemTagsForListVm>();
        public List<GroupItemForListVm> listGroupItems { get; set; } = new List<GroupItemForListVm>();
        public GroupItemForListVm groupItem { get; set; }
        public List<int> listIncomGroupId { get; set; } 
        public int selectedItemTags { get; set; }
        public List<IncomGroupForListVm> listConnectedIncomGroups { get; set; }
        public CategoryForListVm selectedCategory { get; set; }
        public int options { get; set; }
    }
}
