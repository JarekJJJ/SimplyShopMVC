using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SimplyShopMVC.Application.Interfaces;
using SimplyShopMVC.Application.Services;
using SimplyShopMVC.Application.ViewModels.Front;
using SimplyShopMVC.Application.ViewModels.Order;
using SimplyShopMVC.Domain.Interface;

namespace SimplyShopMVC.Web.Components
{
    public class HeaderCartViewComponent: ViewComponent
    {
        private readonly IFrontService _frontService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IOrderService _orderService;

        public HeaderCartViewComponent(IFrontService frontService, UserManager<IdentityUser> userManager, IOrderService orderService)
        {
            _frontService = frontService;
            _userManager = userManager;
            _orderService = orderService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var cartWithItems = new ListCartItemsForListVm();
            var userId = _userManager.GetUserId((System.Security.Claims.ClaimsPrincipal)User);
            if (!string.IsNullOrEmpty(userId))
            {
                cartWithItems = _orderService.GetCartWithCartItems(userId);
                return View(cartWithItems);
            }
            return View(cartWithItems);
        }
    }
}
