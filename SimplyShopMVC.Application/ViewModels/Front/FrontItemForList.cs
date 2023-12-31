﻿using SimplyShopMVC.Application.ViewModels.Item;
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
        public int quantity { get; set; }
        public int deliveryTime { get; set; }
        public int warehouseId { get; set; }
        public List<PhotoItemVm> images { get; set; }
        public List<ItemTagsForListVm>? tags { get; set; }
        public List<OmnibusPriceToListVm> omnibusPriceList { get; set; }
        public int orderQuantity { get; set; }
    }
}
