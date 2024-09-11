using SimplyShopMVC.Application.ViewModels.Item;
using SimplyShopMVC.Application.ViewModels.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Application.ViewModels.Front
{
    public class FrontItemForList
    {
        public int id { get; set; }
        public string name { get; set; }    
        public string? description { get; set; }
        public string? eanCode { get; set; }
        public string itemSymbol { get; set; }
        public string imageFolder { get; set; }
        public decimal priceN { get; set; }
        public decimal priceB { get; set; }
        public decimal priceLevelA { get; set; }
        public decimal priceLevelB { get; set; }
        public decimal priceLevelC { get; set; }
        public int quantity { get; set; }
        public int deliveryTime { get; set; }
        public int warehouseId { get; set; }
        public int? vatRateId { get; set; }
        public List<PhotoItemVm> images { get; set; }
        public List<ItemTagsForListVm>? tags { get; set; }
        public OmnibusPriceToListVm omnibusBestPrice { get; set; }
        public List<OmnibusPriceToListVm> omnibusPriceList { get; set; }
        public int orderQuantity { get; set; }
        public CartItemsForListVm cartItem { get; set; }
    }
}
