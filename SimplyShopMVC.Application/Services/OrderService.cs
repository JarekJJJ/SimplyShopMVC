using AutoMapper;
using AutoMapper.QueryableExtensions;
using SimplyShopMVC.Application.Interfaces;
using SimplyShopMVC.Application.ViewModels.Order;
using SimplyShopMVC.Domain.Interface;
using SimplyShopMVC.Domain.Model.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IMapper _mapper;
        private readonly IItemRepository _itemRepo;
        public OrderService(IOrderRepository orderRepository, IMapper mapper, IItemRepository itemRepository)
        {
            _orderRepo = orderRepository;
            _mapper = mapper;
            _itemRepo = itemRepository;
        }

        public ListCartItemsForListVm AddToCart(CartItemsForListVm cartItem)
        {
            ListCartItemsForListVm listCartItems = new ListCartItemsForListVm();
            var itemWare = _itemRepo.GetAllItemWarehouses().FirstOrDefault(a => a.ItemId == cartItem.ItemId); 
            if(cartItem != null && cartItem.CartId != 0 && itemWare.VatRateId != 0)
            {
                var mappedCartItems = _mapper.Map<CartItems>(cartItem);
                mappedCartItems.VatRateId = itemWare.VatRateId;
                _orderRepo.AddCartItem(mappedCartItems);
                listCartItems = GetCartWithCartItems(cartItem.CartId);
            }
           return listCartItems;
        }

        public ListCartItemsForListVm DeleteCartItemFromCart(CartItemsForListVm cartItems)
        {
            throw new NotImplementedException();
        }

        public ListCartItemsForListVm GetCartWithCartItems(string userId)
        {
            CartForListVm cart = new CartForListVm();
            ListCartItemsForListVm listCartItems= new ListCartItemsForListVm();
            List<CartForListVm> listCart = new List<CartForListVm>();
            var actualCart = _orderRepo.GetAllCarts().FirstOrDefault(a => a.userId == userId && a.IsDeleted == false && a.IsRealized == false && a.IsSaved == false);
            if (actualCart != null)
            {
                var mappedCart = _mapper.Map<CartForListVm>(actualCart);
                cart = mappedCart;
            }
            else
            {
                cart.userId = userId;
                var mappedCart = _mapper.Map<Cart>(cart);
                var cartId = _orderRepo.AddCart(mappedCart);
                var getCart = _orderRepo.GetAllCarts().FirstOrDefault(a => a.Id == cartId);
                cart = _mapper.Map<CartForListVm>(getCart);
            }
            List<CartItemsForListVm> cartItemsForList = new List<CartItemsForListVm>();
            var cartItems = _orderRepo.GetAllCartItems().Where(a => a.CartId == cart.Id)
                .ProjectTo<CartItemsForListVm>(_mapper.ConfigurationProvider).ToList();
            if (cartItems != null)
            {
                cartItemsForList.AddRange(cartItems);
            }
            listCart.Add(cart);
            listCartItems.listCartItems = cartItemsForList;
            listCartItems.listCart = listCart;
            return listCartItems;
        }

        public ListCartItemsForListVm GetCartWithCartItems(int cartId)
        {
            CartForListVm cart = new CartForListVm();
            ListCartItemsForListVm listCartItems = new ListCartItemsForListVm();
            List<CartForListVm> listCart = new List<CartForListVm>();
            var actualCart = _orderRepo.GetAllCarts().FirstOrDefault(a => a.Id == cartId && a.IsDeleted == false && a.IsRealized == false && a.IsSaved == false);
            if (actualCart != null)
            {
                var mappedCart = _mapper.Map<CartForListVm>(actualCart);
                cart = mappedCart;
            }
            //else
            //{
            //    cart.userId = userId;
            //    var mappedCart = _mapper.Map<Cart>(cart);
            //    var cartId = _orderRepo.AddCart(mappedCart);
            //    var getCart = _orderRepo.GetAllCarts().FirstOrDefault(a => a.Id == cartId);
            //    cart = _mapper.Map<CartForListVm>(getCart);
            //}
            List<CartItemsForListVm> cartItemsForList = new List<CartItemsForListVm>();
            var cartItems = _orderRepo.GetAllCartItems().Where(a => a.CartId == cart.Id)
                .ProjectTo<CartItemsForListVm>(_mapper.ConfigurationProvider).ToList();
            if (cartItems != null)
            {
                cartItemsForList.AddRange(cartItems);
            }
            listCart.Add(cart);
            listCartItems.listCartItems = cartItemsForList;
            listCartItems.listCart = listCart;
            return listCartItems;
        }

        public ListCartItemsForListVm OrderCartItemsFromCart(ListCartItemsForListVm listCartItems)
        {
            throw new NotImplementedException();
        }

        public ListCartItemsForListVm SaveCartWithCartItems(ListCartItemsForListVm listCartItems)
        {
            throw new NotImplementedException();
        }
    }
}
