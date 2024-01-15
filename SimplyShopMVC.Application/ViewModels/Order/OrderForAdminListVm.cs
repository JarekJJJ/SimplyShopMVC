using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Application.ViewModels.Order
{
    public class OrderForAdminListVm
    {
        public List<OrderForListVm> listOrders { get; set; }
        public OrderForListVm ordersForListVm { get; set; }
        public List<OrderItemsForListVm> orderItemsForList { get; set; }
        public int selectedOrders { get; set; }
    }
}
