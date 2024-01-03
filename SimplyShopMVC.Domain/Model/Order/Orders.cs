﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Domain.Model.Order
{
    public class Orders
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string NumberOrders { get; set; }
        public string PaymentMethod { get; set; }
        public string DocumentType { get; set; }
        public string ShipingDescription { get; set; }
        public ICollection<OrderItems> OrderItems { get; set; }
    }
}
