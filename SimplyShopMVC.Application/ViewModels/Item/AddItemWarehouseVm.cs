using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Application.ViewModels.Item
{
    public class AddItemWarehouseVm
    {
        public List<ItemWarehouseForListVm> itemWarehouses { get; set; }
        public List<WarehouseForListVm> warehouses { get; set; }
        public List<ItemForListVm> items { get; set; }
        public ItemWarehouseForListVm itemWarehouse { get; set; }
        public List<VatRateForListVm> vatRate { get; set; }
        //public int selectedVatId { get; set; }
        public string searchItem { get; set; }
        public int itemId { get; set; }
        public int selectedWarehouseId { get; set; }
        // dodano 09.12.2023
        public List<CategoryForListVm> categories { get; set; }
        public List<ItemTagsForListVm> itemTags { get; set; }
        public List<ItemTagsForListVm> forDeleteItemTags { get; set; }
        public List<int> selectedItemId { get; set; }
        public int selectDeleteItemTags { get; set; }
        public int selectedItemTag { get; set; }
        public int selectedItemCategory { get; set; }
        public int selectedNewCategory { get; set; }
        public int countItem { get; set; }
    }
}
