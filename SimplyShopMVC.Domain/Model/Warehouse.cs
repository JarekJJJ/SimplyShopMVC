﻿using SimplyShopMVC.Domain.Model.Order;
using SimplyShopMVC.Domain.Model.Suppliers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Domain.Model
{
    public class Warehouse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int DeliveryTime { get; set; }
        public bool IsActive { get; set; }
        public bool? onlyRegistered { get; set; } = true;

        public virtual ICollection<ItemWarehouse> ItemWarehouses { get; set; }
        public virtual ICollection<Incom> Incoms { get; set; }
        public virtual ICollection<Orink> Orinks { get; set; }
        public virtual ICollection<OmnibusPrice> OmnibusPrices { get; set;}
        public virtual ICollection<CartItems> CartItems { get; set; }
        public ICollection<OrderItems> OrderItems { get; set; }
    }
}
