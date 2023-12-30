using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Application.ViewModels.Order
{
    public class ListCartItemsForListVm
    {
        public List<CartForListVm> listCart { get; set; }
        public List<CartItemsForListVm> listCartItems { get; set; }
    }
}
