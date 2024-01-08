using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        public IActionResult Index()
        {
            var userId = _userManager.GetUserId(User);
            if(!string.IsNullOrEmpty(userId) )
            {
                var cartWithItems = _orderService.GetCartWithCartItems(userId);
                return View(cartWithItems);
            }
           return View();
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
        }
        [HttpPost]
        public IActionResult DeleteCartItem(int cartItemId, int cartId)
        {
            _orderService.DeleteCartItemFromCart(cartItemId, cartId);
            var userId = _userManager.GetUserId(User);
            var cartWithItems = _orderService.GetCartWithCartItems(userId);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult EditCartItem (int cartItemId, int quantity, int cartId)
        {
            _orderService.UpdateCartItem(cartItemId, quantity, cartId);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult SaveCart (CartForListVm cart) // Funkcja zapsywania koszyków do dodania w przyszłości.
        {
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult OrderView(int cartId)
        {
            var cartWithItems = _orderService.GetCartWithCartItems(cartId);
            var orderItems = _orderService.SendOrderFromCart(cartWithItems);
            return View(orderItems);
        }
        [HttpPost]
        [Authorize]
        public IActionResult finishOrder(OrderFromCartVm _orderForList)
        {
            var newOrderId = _orderService.AddOrder(_orderForList);
            var orderFinished = _orderService.FinishOrder(_orderForList,newOrderId);


            return RedirectToAction("Index");
        }
        [HttpGet,Authorize]
        public IActionResult listOrderForUser()
        {
            var userId = _userManager.GetUserId(User);
            var userOrder = _orderService.GetOrdersByUserId(userId);
            return View(userOrder);
        } // Do zrobienia akcja z pobieraniem pdf nazwa akcji "GetPdfDocument"
    }
}
