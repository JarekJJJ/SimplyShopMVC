using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Application.ViewModels.Item
{
    public class UpdateWarehouseVm
    {
        public List<WarehouseForListVm> listWarehouse { get;set; }
        public WarehouseForListVm warehouse { get;set; }
        public List<CountWarehouseVm> countWarehouse { get;set; }
        public string searchWarehouse { get;set; }
    }
}
