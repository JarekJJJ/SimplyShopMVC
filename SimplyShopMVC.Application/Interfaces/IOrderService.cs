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
        ListCartItemsForListVm UpdateCartItem(int cartItemId, int quantity, int cartId);
        ListCartItemsForListVm DeleteCartItemFromCart(int cartItemId, int cartId);
        ListCartItemsForListVm OrderCartItemsFromCart(ListCartItemsForListVm listCartItems);
        ListCartItemsForListVm SaveCartWithCartItems(ListCartItemsForListVm listCartItems);
        OrderFromCartVm SendOrderFromCart(ListCartItemsForListVm listCartItems);
        OrderFromCartVm FinishOrder(OrderFromCartVm orderForList, int _orderId);
        int AddOrder(OrderFromCartVm result);
        OrderForUserListVm GetOrdersByUserId(string userId);

    }
}
