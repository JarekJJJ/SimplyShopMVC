using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimplyShopMVC.Application.Interfaces;
using SimplyShopMVC.Application.ViewModels.Front;
using SimplyShopMVC.Application.ViewModels.Order;
using SimplyShopMVC.Domain.Interface;
using System.Drawing.Printing;

namespace SimplyShopMVC.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IFrontService _frontService;
        private readonly UserManager<IdentityUser> _userManager;
        public CartController(IOrderService orderService, UserManager<IdentityUser> userManager, IFrontService frontService)
        {
            _orderService = orderService;
            _userManager = userManager;
            _frontService = frontService;
        }

        public IActionResult Index()
        {
            var userId = _userManager.GetUserId(User);
            var cartWithItems = _orderService.GetCartWithCartItems(userId);
            return View(cartWithItems);
        }
        [HttpPost]
        public IActionResult AddCartItem(ListItemShopIndexVm result)
        {
            var userId = _userManager.GetUserId(User);
            
            if (result.searchItem is null)
            {
                result.searchItem = string.Empty;
            }          
            _orderService.AddToCart(result.cartItem);
            var backResult = _frontService.GetItemsByCategory(result.selectedCategory, result.pageSize, result.currentPage, result.searchItem, result.selectedTag, userId);
            var receivedCategories = _frontService.GetAllCategories();
            backResult.categories = receivedCategories.categories.ToList();
            return View("~/Views/Item/Index.cshtml", backResult);
            // zrobić cenę netto
            // widok koszyka !!!
        }

    }
}
