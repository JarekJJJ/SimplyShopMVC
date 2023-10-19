using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Application.ViewModels.Item
{
    public class AddItemWarehouseVm
    {
        public ItemWarehouseForListVm itemWarehouse { get; set; }
        public List<WarehouseForListVm> warehouses { get; set; }
        public List<ItemForListVm> items { get; set; }
        public string searchItem { get; set; }
        public int selectedWarehouseId { get; set; }
    }
}
