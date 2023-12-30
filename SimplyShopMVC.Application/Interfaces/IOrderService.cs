using SimplyShopMVC.Application.ViewModels.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Application.Interfaces
{
    public interface IOrderService
    {
        ListCartItemsForListVm AddToCart(CartItemsForListVm cartItems);
        ListCartItemsForListVm GetCartWithCartItems(string userId);
        ListCartItemsForListVm GetCartWithCartItems(int cartId);
        ListCartItemsForListVm DeleteCartItemFromCart(CartItemsForListVm cartItems);
        ListCartItemsForListVm OrderCartItemsFromCart(ListCartItemsForListVm listCartItems);
        ListCartItemsForListVm SaveCartWithCartItems(ListCartItemsForListVm listCartItems);

    }
}
