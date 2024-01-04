using SimplyShopMVC.Domain.Model.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Domain.Interface
{
    public interface IOrderRepository
    {
        int AddCart(Cart cart);
        IQueryable<Cart> GetAllCarts();
        void UpdateCart(Cart cart);
        void DeleteCart(int cartId);
        int AddCartItem(CartItems cartItem);
        IQueryable<CartItems> GetAllCartItems();
        void UpdateCartItem(CartItems cartItem);
        void DeleteCartItem(int cartItemId);
        int AddOrders(Orders orders);
        void UpdateOrders(Orders orders);
        void DeleteOrders(int orderId);
        IQueryable<Orders> GetAllOrders();
        int AddOrderItems(OrderItems ordersItems);
        void UpdateOrderItems(OrderItems ordersItems);
        void DeleteOrderItems(int orderItemId);
        IQueryable<OrderItems> GetAllOrderItems();



    }
}
