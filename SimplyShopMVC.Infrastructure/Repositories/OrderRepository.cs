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
    }
}
