using SimplyShopMVC.Domain.Interface;
using SimplyShopMVC.Domain.Model.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly Context _context;
        public OrderRepository(Context context)
        {
            _context = context;
        }

        public int AddCart(Cart cart)
        {
            _context.Carts.Add(cart);
            _context.SaveChanges();
            return cart.Id;
        }

        public int AddCartItem(CartItems cartItem)
        {
            _context.CartsItems.Add(cartItem);
            _context.SaveChanges();
            return cartItem.Id;
        }

        public int AddOrderItems(OrderItems ordersItems)
        {
            _context.OrderItems.Add(ordersItems);
            _context.SaveChanges();
            return ordersItems.Id;
        }

        public int AddOrders(Orders orders)
        {
           _context.Orders.Add(orders);
            _context.SaveChanges();
            return orders.Id;
        }

        public void DeleteCart(int cartId)
        {
            var cartToRemove = _context.Carts.Find(cartId);
            if (cartToRemove != null)
            {
                _context.Carts.Remove(cartToRemove);
                _context.SaveChanges();
            }
        }

        public void DeleteCartItem(int cartItemId)
        {
            var cartItemToRemove = _context.CartsItems.Find(cartItemId);
            if (cartItemToRemove != null)
            {
                _context.CartsItems.Remove(cartItemToRemove);
                _context.SaveChanges();
            }
        }

        public void DeleteOrderItems(int orderItemId)
        {
            var orderItemToRemove = _context.OrderItems.Find(orderItemId);
            if (orderItemToRemove != null)
            {
                _context.OrderItems.Remove(orderItemToRemove);
                _context.SaveChanges();
            }
        }

        public void DeleteOrders(int orderId)
        {
            var ordersToRemove = _context.Orders.Find(orderId);
            if(ordersToRemove!=null)
            {
                _context.Orders.Remove(ordersToRemove);
                _context.SaveChanges();
            }
        }

        public IQueryable<CartItems> GetAllCartItems()
        {
            var cartItems = _context.CartsItems;
            return cartItems;
        }

        public IQueryable<Cart> GetAllCarts()
        {
            var carts = _context.Carts;
            return carts;
        }

        public IQueryable<OrderItems> GetAllOrderItems()
        {
            var orderItems = _context.OrderItems;
            return orderItems;
        }

        public IQueryable<Orders> GetAllOrders()
        {
            var orders = _context.Orders;
            return orders;
        }

        public void UpdateCart(Cart cart)
        {
            _context.Carts.Update(cart);
            _context.SaveChanges();
        }

        public void UpdateCartItem(CartItems cartItem)
        {
            _context.CartsItems.Update(cartItem);
            _context.SaveChanges();
        }

        public void UpdateOrderItems(OrderItems ordersItems)
        {
           _context.OrderItems.Update(ordersItems);
            _context.SaveChanges();
        }

        public void UpdateOrders(Orders orders)
        {
            _context.Orders.Update(orders);
            _context.SaveChanges();
        }
    }
}
