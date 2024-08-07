﻿using SimplyShopMVC.Application.ViewModels.Order;
using SimplyShopMVC.Application.ViewModels.user;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Application.Interfaces
{
    public interface IOrderService
    {
        ListDeliveryForListVm GetAllDeliveryToList();
        Task UpdateDelivery(ListDeliveryForListVm listDelivery);
        void AddDelivery(ListDeliveryForListVm listDelivery);
        void DeleteDelivery(ListDeliveryForListVm listDelivery);
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
        byte[] GetPdfDocumentFromService(int _orderId);
        OrderForAdminListVm GetOrdersForAdmin(int status, string? userId, int? orderId, int options, string searchString);
        OrderForAdminListVm ViewOrderForAdmin(int? orderId, string? userId);
        void AdminFinishOrder(OrderForAdminListVm result, int options);
        UserDetailForListVm GetUserDetailById(string userId);
        int AddFavoriteItemToCart(int itemId, int quantity, string userId);
    }
}
