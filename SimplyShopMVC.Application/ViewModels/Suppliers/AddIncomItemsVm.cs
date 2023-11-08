using Microsoft.AspNetCore.Http;
using SimplyShopMVC.Application.ViewModels.Item;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Application.ViewModels.Suppliers
{
    public class AddIncomItemsVm
    {
        public List<WarehouseForListVm> warehouseForListVm { get; set; }
        public int warehouseId { get; set; }
        public IFormFile formFile { get; set; }
        public List<string> raportAddItem { get; set; }
        public bool removeItems { get; set; }
    }
}
