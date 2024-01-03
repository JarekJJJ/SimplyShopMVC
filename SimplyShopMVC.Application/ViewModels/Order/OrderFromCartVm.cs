using SimplyShopMVC.Application.ViewModels.user;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Application.ViewModels.Order
{
    public class OrderFromCartVm
    {
        public List<CartItemsForListVm> cartItems { get; set; }
        public List<OrderItemsForListVm> orderItems { get; set; }
        public OrderForListVm orderForList { get; set; }
        public UserDetailForListVm userDetail { get; set; }
    }
}
