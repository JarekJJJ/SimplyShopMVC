using AutoMapper;
using AutoMapper.QueryableExtensions;
using SimplyShopMVC.Application.Helpers;
using SimplyShopMVC.Application.Interfaces;
using SimplyShopMVC.Application.ViewModels.Order;
using SimplyShopMVC.Application.ViewModels.user;
using SimplyShopMVC.Domain.Interface;
using SimplyShopMVC.Domain.Model.Order;
using SimplyShopMVC.Domain.Model.users;
using SimplyShopMVC.Infrastructure.Migrations;
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
        private readonly IPriceCalculate priceCalc;
        private readonly IDeliveryRepository _deliveryRepo;
        private readonly IFrontService _frontService;
        public OrderService(IOrderRepository orderRepository, IMapper mapper, IItemRepository itemRepository, IUserRepository userRepository, IEmailService sendEmail, IGeneratePdf genPdf, IPriceCalculate priceCalculate
        , IDeliveryRepository deliveryRepo, IFrontService frontService)
        {
            _orderRepo = orderRepository;
            _mapper = mapper;
            _itemRepo = itemRepository;
            _userRepo = userRepository;
            _sendEmail = sendEmail;
            _genPdf = genPdf;
            priceCalc = priceCalculate;
            _deliveryRepo = deliveryRepo;
            _frontService = frontService;
        }
        public ListDeliveryForListVm GetAllDeliveryToList() // Kontroler shop - deliverySettings - HttpGet
        {
            ListDeliveryForListVm listDelivery = new ListDeliveryForListVm();
            var result = _deliveryRepo.GetAllDeliveries().ProjectTo<DeliveryForListVm>(_mapper.ConfigurationProvider).ToList();
            listDelivery.listDelivery = result;
            return listDelivery;
        }
        public async Task UpdateDelivery(ListDeliveryForListVm listDelivery) // shop controller - Delivery settings - Post - options: 2;
        {
            if (listDelivery.delivery != null)
            {
                await _deliveryRepo.UpdateDeliveryAsync(_mapper.Map<Delivery>(listDelivery.delivery));
            }
        }
        public void AddDelivery(ListDeliveryForListVm listDelivery) // shop controller - Delivery settings - Post - options: 1;
        {
            if (listDelivery.delivery != null)
            {
                _deliveryRepo.AddDelivery(_mapper.Map<Delivery>(listDelivery.delivery));
            }
        }
        public void DeleteDelivery(ListDeliveryForListVm listDelivery) // shop controller - Delivery settings - Post - options: 3;
        {
            if (listDelivery.delivery != null)
            {
                 _deliveryRepo.DeleteDelivery(listDelivery.delivery.Id);
            }
        }
        public ListCartItemsForListVm AddToCart(CartItemsForListVm cartItem)
        {
            ListCartItemsForListVm listCartItems = new ListCartItemsForListVm();
            var itemWare = _itemRepo.GetAllItemWarehouses().FirstOrDefault(a => a.ItemId == cartItem.ItemId);
            if (cartItem != null && cartItem.CartId != 0 && itemWare.VatRateId != 0 && itemWare.Quantity > 0)
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
            foreach (var cartItem in cartItemsForList)
            {
                var item = _itemRepo.GetAllItemWarehouses().FirstOrDefault(i => i.ItemId == cartItem.ItemId && i.WarehouseId == cartItem.WarehouseId);
                if (item != null)
                {
                    cartItem.PriceN = priceCalc.priceCalc(cartItem.ItemId, cartItem.WarehouseId, userId);
                }
            }
            listCart.Add(cart);
            listCartItems.listCartItems = cartItemsForList;
            listCartItems.listCart = listCart;
            listCartItems.countCartItems = listCartItems.listCartItems.Count;
            return listCartItems;
        }

        public ListCartItemsForListVm GetCartWithCartItems(int cartId)
        {
            CartForListVm cart = new CartForListVm();
            ListCartItemsForListVm listCartItems = new ListCartItemsForListVm();
            List<CartForListVm> listCart = new List<CartForListVm>();
            UserDetail userDetail = new UserDetail();
            var actualCart = _orderRepo.GetAllCarts().FirstOrDefault(a => a.Id == cartId && a.IsDeleted == false && a.IsRealized == false && a.IsSaved == false);
            if (actualCart != null)
            {
                var mappedCart = _mapper.Map<CartForListVm>(actualCart);
                cart = mappedCart;
                userDetail = _userRepo.GetAllUsers().FirstOrDefault(u => u.UserId == actualCart.userId);
            }
            List<CartItemsForListVm> cartItemsForList = new List<CartItemsForListVm>();
            var cartItems = _orderRepo.GetAllCartItems().Where(a => a.CartId == cart.Id)
                .ProjectTo<CartItemsForListVm>(_mapper.ConfigurationProvider).ToList();
            if (cartItems != null)
            {
                cartItemsForList.AddRange(cartItems);
            }
            foreach (var cartItem in cartItemsForList)
            {
                var item = _itemRepo.GetAllItemWarehouses().FirstOrDefault(i => i.ItemId == cartItem.ItemId && i.WarehouseId == cartItem.WarehouseId);
                if (item != null)
                {
                    cartItem.PriceN = priceCalc.priceCalc(cartItem.ItemId, cartItem.WarehouseId, userDetail.UserId);
                }
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
            orderFromCart.listDelivery = new List<DeliveryForListVm>();
            var userFromCart = listCartItems.listCart.FirstOrDefault();
            var userDetail = _userRepo.GetAllUsers().FirstOrDefault(a => a.UserId == userFromCart.userId);
            orderFromCart.listDelivery = _deliveryRepo.GetAllDeliveries().ProjectTo<DeliveryForListVm>(_mapper.ConfigurationProvider).ToList();

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
            var userDetail = _userRepo.GetAllUsers().FirstOrDefault(u => u.UserId == result.userDetail.UserId);
            if (userDetail != null && !String.IsNullOrEmpty(userDetail.FullName))
            {
                mappedOrder.UserName = userDetail.FullName;
            }
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
                var itemForList = _itemRepo.GetAllItems().FirstOrDefault(i => i.Id == cartItem.ItemId);
                OrderItemsForListVm orderItem = new OrderItemsForListVm();
                orderItem.OrdersId = _orderId;
                orderItem.ItemId = cartItem.ItemId;
                if (itemForList != null && !string.IsNullOrEmpty(itemForList.EanCode))
                {
                    orderItem.EanCode = itemForList.EanCode;
                }
                else
                {
                    orderItem.EanCode = string.Empty;
                }
                orderItem.Quantity = cartItem.Quantity;
                orderItem.VatRateId = cartItem.VatRateId;
                orderItem.Name = cartItem.Name;
                orderItem.PriceB = cartItem.PriceN; //pomyłka w nazwie PrizeN to cena brutto
                orderItem.WarehouseId = cartItem.WarehouseId;
                var mappedOrderItems = _mapper.Map<OrderItems>(orderItem);
                var orderItemId = _orderRepo.AddOrderItems(mappedOrderItems);
                orderItem.Id = orderItemId;
                newOrder.orderItems.Add(orderItem);
            }
            newOrder.userDetail = _mapper.Map<UserDetailForListVm>(_userRepo.GetAllUsers().FirstOrDefault(u => u.UserId == orderForList.userDetail.UserId));
            var cartToUpdate = _orderRepo.GetAllCarts().FirstOrDefault(c => c.Id == orderForList.cartIdToOrder);
            if (cartToUpdate != null)
            {
                cartToUpdate.RealizedDate = DateTime.Now;
                cartToUpdate.IsRealized = true;
                cartToUpdate.IsDeleted = true;
                _orderRepo.UpdateCart(cartToUpdate);
            }
            newOrder.cartItems = listCartItem;
            newOrder.orderForList = orderForList.orderForList;
            var orderPdf = _genPdf.GenertateOrderPdf(newOrder);
            _sendEmail.SendEmail($"{newOrder.userDetail.EmailAddress}", "test", $"Złożono zamówienie nr: {orderForList.orderForList.NumberOrders}", orderPdf);

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
        public OrderForAdminListVm GetOrdersForAdmin(int status, string? userId, int? orderId, int options, string searchString)
        {
            OrderForAdminListVm orderForList = new OrderForAdminListVm();
            orderForList.listOrders = new List<OrderForListVm>();
            if (status == 0)
            {
                //if (!string.IsNullOrEmpty(userId))
                //{
                //    var repoUserOrders = _orderRepo.GetAllOrders().Where(r => r.UserId == userId)
                //        .ProjectTo<OrderForListVm>(_mapper.ConfigurationProvider).ToList();
                //    orderForList.listOrders.AddRange(repoUserOrders);
                //    return orderForList;
                //}
                if (!String.IsNullOrEmpty(searchString) || options > 0)
                {
                    if (options > 0)
                    {
                        DateTime ordersTime = DateTime.Now.AddDays(options * (-1));
                        var selectedOrders = _orderRepo.GetAllOrders().Where(o => (o.UserName.Contains(searchString) || o.NumberOrders.Contains(searchString)) && (o.CreatedDate >= ordersTime))
                            .ProjectTo<OrderForListVm>(_mapper.ConfigurationProvider).ToList();
                        orderForList.listOrders.AddRange(selectedOrders);
                    }
                    else
                    {
                        var selectedOrders = _orderRepo.GetAllOrders().Where(o => o.UserName.Contains(searchString) || o.NumberOrders.Contains(searchString))
                            .ProjectTo<OrderForListVm>(_mapper.ConfigurationProvider).ToList();
                        orderForList.listOrders.AddRange(selectedOrders);
                    }

                    return orderForList;
                }
                var repoAllOrders = _orderRepo.GetAllOrders()
                       .ProjectTo<OrderForListVm>(_mapper.ConfigurationProvider).Take(100).ToList();
                orderForList.listOrders.AddRange(repoAllOrders);
                return orderForList;
            }
            var orderToChange = _orderRepo.GetAllOrders().FirstOrDefault(o => o.Id == orderId);
            string subject = string.Empty;
            string message = string.Empty;
            byte[] orderPdf;
            if (orderToChange != null)
            {
                switch ((status, orderId))
                {
                    case (1, > 0):
                        orderToChange.IsAccepted = true;
                        orderToChange.IsCancelled = false;
                        _orderRepo.UpdateOrders(orderToChange);
                        orderPdf = GetPdfDocumentFromService((int)orderId);
                        subject = "Zaakceptowano złożone zamówienie nr:";
                        message = "Zaakceptowano złożone zamówienie, szczegóły w załączniku";
                        SentEmailWithOrdersDetail((int)orderId, subject, message, orderPdf);
                        break;
                    case (2, > 0):
                        break;
                    case (3, > 0):
                        orderToChange.IsCancelled = true;
                        orderToChange.IsAccepted = false;
                        orderToChange.IsCompleted = false;
                        _orderRepo.UpdateOrders(orderToChange);
                        orderPdf = GetPdfDocumentFromService((int)orderId);
                        subject = "anulowano złożone zamówienie nr:";
                        message = "anulowano złożone zamówienie, szczegóły w załączniku";
                        SentEmailWithOrdersDetail((int)orderId, subject, message, orderPdf);
                        break;
                    case (4, > 0):
                        orderToChange.IsAccepted = false;
                        orderToChange.IsCompleted = false;
                        orderToChange.IsCancelled = false;
                        _orderRepo.UpdateOrders(orderToChange);
                        orderPdf = GetPdfDocumentFromService((int)orderId);
                        subject = "zmiana statusu zamówienia!";
                        message = "Zamówienie ponownie oczekuje na akceptacje, szczegóły w załączniku.";
                        SentEmailWithOrdersDetail((int)orderId, subject, message, orderPdf);
                        break;
                    default: break;
                }
            }

            return orderForList;
        }
        public OrderForAdminListVm ViewOrderForAdmin(int? orderId, string? userId)
        {
            OrderForAdminListVm ordersList = new OrderForAdminListVm();
            ordersList.orderItemsForList = new List<OrderItemsForListVm> { new OrderItemsForListVm() };
            ordersList.userDetail = new UserDetailForListVm();
            ordersList.ordersForListVm = new OrderForListVm();
            var order = _mapper.Map<OrderForListVm>(_orderRepo.GetAllOrders().FirstOrDefault(o => o.Id == orderId));
            ordersList.ordersForListVm = order;
            var orderItemsList = _orderRepo.GetAllOrderItems().Where(i => i.OrdersId == orderId)
                .ProjectTo<OrderItemsForListVm>(_mapper.ConfigurationProvider).ToList();
            ordersList.orderItemsForList = orderItemsList;
            var userForVm = _mapper.Map<UserDetailForListVm>(_userRepo.GetAllUsers().FirstOrDefault(u => u.UserId == userId));
            ordersList.userDetail = userForVm;

            return ordersList;
        }
        public void AdminFinishOrder(OrderForAdminListVm result, int options)
        {
            OrderForAdminListVm newOrder = new OrderForAdminListVm();
            switch (options)
            {
                case 1:
                    if (result.orderItem != null)
                    {
                        var orderItem = _orderRepo.GetAllOrderItems().FirstOrDefault(o => o.Id == result.orderItem.Id);
                        if (orderItem != null)
                        {
                            orderItem.Quantity = result.orderItem.Quantity;
                            orderItem.PriceB = result.orderItem.PriceB;
                            _orderRepo.UpdateOrderItems(orderItem);
                        }

                    }
                    break;
                default:
                    _userRepo.UpdateUserDetail(_mapper.Map<UserDetail>(result.userDetail));
                    var mappedOrder = _mapper.Map<Orders>(result.ordersForListVm);
                    _orderRepo.UpdateOrders(mappedOrder);
                    break;
            }




        }
        public byte[] GetPdfDocumentFromService(int _orderId)
        {
            OrderFromCartVm orderFromCart = new OrderFromCartVm();
            var orderToPdf = _mapper.Map<OrderForListVm>(_orderRepo.GetAllOrders().FirstOrDefault(o => o.Id == _orderId));

            var orderItemsToPdf = _orderRepo.GetAllOrderItems().Where(i => i.OrdersId == _orderId)
                .ProjectTo<OrderItemsForListVm>(_mapper.ConfigurationProvider).ToList();
            var userDetail = _mapper.Map<UserDetailForListVm>(_userRepo.GetAllUsers().FirstOrDefault(u => u.UserId == orderToPdf.UserId));
            orderFromCart.orderForList = orderToPdf;
            orderFromCart.orderItems = orderItemsToPdf;
            orderFromCart.userDetail = userDetail;
            var orderPdf = _genPdf.GenertateOrderPdf(orderFromCart);
            return orderPdf;
        }
        public bool SentEmailWithOrdersDetail(int orderId, string subject, string message, byte[] pdfDoc)
        {
            var newOrder = _orderRepo.GetAllOrders().FirstOrDefault(o => o.Id == orderId);
            UserDetail userDetail = new UserDetail();
            if (newOrder != null)
            {
                userDetail = _userRepo.GetAllUsers().FirstOrDefault(o => o.UserId == newOrder.UserId);
            }
            if (userDetail != null && !String.IsNullOrEmpty(userDetail.EmailAddress))
            {
                _sendEmail.SendEmail($"{userDetail.EmailAddress}", subject, message, pdfDoc);

            }
            return true;
        }
        public UserDetailForListVm GetUserDetailById(string userId)
        {
            var userDetail = _mapper.Map<UserDetailForListVm>(_userRepo.GetAllUsers().FirstOrDefault(u => u.UserId == userId));
            return userDetail;
        }
        public int AddFavoriteItemToCart(int itemId, int quantity, string userId)
        {
            CartItemsForListVm cartItem = new CartItemsForListVm();
            if(itemId> 0 && quantity >0 && !String.IsNullOrEmpty(userId))
            {
                cartItem.ItemId = itemId;
                cartItem.Quantity = quantity;
                var cartId = _frontService.GetCart(userId);
                if(cartId != null)
                {
                    cartItem.CartId = cartId.Id;
                    var itemDetail = _frontService.GetItemDetail(itemId, userId);
                    if (itemDetail != null)
                    {
                        cartItem.PriceN = itemDetail.priceB;
                       // cartItem.VatRateId = (int)itemDetail.vatRateId; // vatRateId jest pobierane w AddToCart ! 
                        cartItem.WarehouseId = itemDetail.warehouseId;
                        cartItem.Name = itemDetail.name;
                        AddToCart(cartItem);
                        return 1;
                    }
                }
                
            }
            return 0;
        }
    }
}
