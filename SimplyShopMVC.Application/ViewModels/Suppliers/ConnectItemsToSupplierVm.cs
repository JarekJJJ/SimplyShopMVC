using SimplyShopMVC.Application.ViewModels.Item;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Application.ViewModels.Suppliers
{
    public class ConnectItemsToSupplierVm
    {
        public List<ItemWarehouseForListVm> itemsWarehouse { get; set; }
        public List<WarehouseForListVm> warehouseForLists { get; set; }
        public List<IncomGroupForListVm> incomGroups { get; set; }
        public List<GroupItemForListVm> groupItems { get; set; }
        public List<CategoryForListVm> categoryItems { get; set; }
        public List<ItemTagsForListVm> itemTagsForLists { get; set; }
        public CategoryForListVm category { get; set; }
        public GroupItemForListVm groupItem { get; set; }
        public ItemTagsForListVm itemTag { get; set; }
        public List<string> raport { get; set; }
        public List<CountSupplierItem> countSupplierItems { get; set; }
        public List<int> selectedItemTags { get; set; }
        public int? selectedWarehouse { get; set; }
        public int? selectedGroupItem { get; set; }
        public int? selectedCategory { get; set; }
        public int? selectedGroupSupplier { get; set; }
    }
}
