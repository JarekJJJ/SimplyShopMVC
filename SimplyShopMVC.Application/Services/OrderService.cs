using AutoMapper;
using AutoMapper.QueryableExtensions;
using SimplyShopMVC.Application.Helpers;
using SimplyShopMVC.Application.Interfaces;
using SimplyShopMVC.Application.ViewModels.Order;
using SimplyShopMVC.Application.ViewModels.user;
using SimplyShopMVC.Domain.Interface;
using SimplyShopMVC.Domain.Model.Order;
using SimplyShopMVC.Domain.Model.users;
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
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;
        private readonly IItemRepository _itemRepo;
        private readonly IEmailService _sendEmail;
        private readonly IGeneratePdf _genPdf;
        public OrderService(IOrderRepository orderRepository, IMapper mapper, IItemRepository itemRepository, IUserRepository userRepository, IEmailService sendEmail, IGeneratePdf genPdf)
        {
            _orderRepo = orderRepository;
            _mapper = mapper;
            _itemRepo = itemRepository;
            _userRepo = userRepository;
            _sendEmail = sendEmail;
            _genPdf = genPdf;
        }

        public ListCartItemsForListVm AddToCart(CartItemsForListVm cartItem)
        {
            ListCartItemsForListVm listCartItems = new ListCartItemsForListVm();
            var itemWare = _itemRepo.GetAllItemWarehouses().FirstOrDefault(a => a.ItemId == cartItem.ItemId);
            if (cartItem != null && cartItem.CartId != 0 && itemWare.VatRateId != 0)
            {
                var mappedCartItems = _mapper.Map<CartItems>(cartItem);
                mappedCartItems.VatRateId = itemWare.VatRateId;
                _orderRepo.AddCartItem(mappedCartItems);
                listCartItems = GetCartWithCartItems(cartItem.CartId);
            }
            return listCartItems;
        }

        public ListCartItemsForListVm DeleteCartItemFromCart(int cartItemId, int cartId)
        {
            if (cartItemId != 0 && cartId != 0)
            {
                _orderRepo.DeleteCartItem(cartItemId);
            }
            ListCartItemsForListVm listCartItems = new ListCartItemsForListVm();
            listCartItems = GetCartWithCartItems(cartId);
            return listCartItems;
        }       

        public ListCartItemsForListVm GetCartWithCartItems(string userId)
        {
            CartForListVm cart = new CartForListVm();
            ListCartItemsForListVm listCartItems = new ListCartItemsForListVm();
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

        public OrderFromCartVm SendOrderFromCart(ListCartItemsForListVm listCartItems)
        {
            OrderFromCartVm orderFromCart = new OrderFromCartVm();
            orderFromCart.cartItems = listCartItems.listCartItems;
            orderFromCart.orderForList = new OrderForListVm();
            var userFromCart = listCartItems.listCart.FirstOrDefault();
            var userDetail = _userRepo.GetAllUsers().FirstOrDefault(a => a.UserId == userFromCart.userId);

            if (userDetail == null)
            {
                UserDetail newUserDetail = new UserDetail();
                newUserDetail.UserId = userFromCart.userId;
                _userRepo.AddUserDetail(newUserDetail);
                orderFromCart.userDetail = _mapper.Map<UserDetailForListVm>(newUserDetail);
            }
            else
            {
                orderFromCart.userDetail = _mapper.Map<UserDetailForListVm>(userDetail);
            }
            var orderToView = _orderRepo.GetAllOrders().Where(o => o.UserId == userDetail.UserId).Count();
            orderFromCart.orderForList.NumberOrders = ($"{orderToView + 1}/{userDetail.Id}/{DateTime.Now.Year}");
            return orderFromCart;
        }

        public ListCartItemsForListVm UpdateCartItem(int cartItemId, int quantity, int cartId)
        {
            if (quantity <= 0)
            {
                quantity = 1;
            }
            var resultCartItem = _orderRepo.GetAllCartItems().FirstOrDefault(a => a.Id == cartItemId);
            if (resultCartItem != null)
            {
                resultCartItem.Quantity = quantity;
                _orderRepo.UpdateCartItem(resultCartItem);
            }
            var refreshCart = GetCartWithCartItems(cartId);
            return refreshCart;
        }
        public int AddOrder(OrderFromCartVm result)
        {
            if (String.IsNullOrEmpty(result.orderForList.ShipingDescription))
            {
                result.orderForList.ShipingDescription = "brak";
            }
            var mappedOrder = _mapper.Map<Orders>(result.orderForList);
            var _orderId = _orderRepo.AddOrders(mappedOrder);
            return _orderId;
        }
        public OrderFromCartVm FinishOrder(OrderFromCartVm orderForList, int _orderId)
        {
            OrderFromCartVm newOrder = new OrderFromCartVm();
            newOrder.orderItems = new List<OrderItemsForListVm>();
           
            _userRepo.UpdateUserDetail(_mapper.Map<UserDetail>(orderForList.userDetail));
            var listCartItem = _orderRepo.GetAllCartItems().Where(c => c.CartId == orderForList.cartIdToOrder)
                .ProjectTo<CartItemsForListVm>(_mapper.ConfigurationProvider).ToList();
            foreach (var cartItem in listCartItem)
            {
                OrderItemsForListVm orderItem = new OrderItemsForListVm();
                orderItem.OrdersId = _orderId;
                orderItem.ItemId = cartItem.ItemId;
                orderItem.Quantity= cartItem.Quantity;
                orderItem.VatRateId= cartItem.VatRateId;
                orderItem.Name= cartItem.Name;
                orderItem.PriceB = cartItem.PriceN; //pomyłka w nazwie PrizeN to cena brutto
                orderItem.WarehouseId= cartItem.WarehouseId;
                var mappedOrderItems = _mapper.Map<OrderItems>(orderItem);
                var orderItemId = _orderRepo.AddOrderItems(mappedOrderItems);
                orderItem.Id= orderItemId;
                newOrder.orderItems.Add(orderItem);              
            }
            newOrder.userDetail = _mapper.Map<UserDetailForListVm>(_userRepo.GetAllUsers().FirstOrDefault(u => u.UserId == orderForList.userDetail.UserId));
            var cartToUpdate = _orderRepo.GetAllCarts().FirstOrDefault(c=>c.Id==orderForList.cartIdToOrder);
           if(cartToUpdate != null)
            {
                cartToUpdate.RealizedDate = DateTime.Now;
                cartToUpdate.IsRealized = true;
                cartToUpdate.IsDeleted = true;
                _orderRepo.UpdateCart(cartToUpdate);
            }
           var orderPdf = _genPdf.GenertateOrderPdf(orderForList);
            _sendEmail.SendEmail($"{orderForList.userDetail.EmailAddress}","test",$"Złożono zamówienie nr: {orderForList.orderForList.NumberOrders}",orderPdf);
 
            return newOrder;
        }
        public OrderForUserListVm GetOrdersByUserId(string userId)
        {
            OrderForUserListVm orderForUserList = new OrderForUserListVm();
            orderForUserList.listUserOrders = new List<OrderForListVm>();
            var repoUserOrders = _orderRepo.GetAllOrders().Where(r => r.UserId == userId)
                .ProjectTo<OrderForListVm>(_mapper.ConfigurationProvider).ToList();
            orderForUserList.listUserOrders.AddRange(repoUserOrders);

            return orderForUserList;
        }

        public byte[] GetPdfDocumentFromService(int _orderId)
        {
            OrderFromCartVm orderFromCart = new OrderFromCartVm();
            var orderToPdf = _mapper.Map<OrderForListVm>(_orderRepo.GetAllOrders().FirstOrDefault(o => o.Id == _orderId));
                 
            var orderItemsToPdf = _orderRepo.GetAllOrderItems().Where(i=>i.OrdersId== _orderId)
                .ProjectTo<OrderItemsForListVm>(_mapper.ConfigurationProvider).ToList();
            var userDetail = _mapper.Map<UserDetailForListVm>(_userRepo.GetAllUsers().FirstOrDefault(u => u.UserId == orderToPdf.UserId));
            orderFromCart.orderForList = orderToPdf;
            orderFromCart.orderItems = orderItemsToPdf;
            orderFromCart.userDetail = userDetail;
            var orderPdf = _genPdf.GenertateOrderPdf(orderFromCart);
            return orderPdf;
        }
    }
}
