using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimplyShopMVC.Application.Interfaces;
using SimplyShopMVC.Application.ViewModels;
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
        private readonly IWebHostEnvironment _webHostEnvironment;
        public CartController(IOrderService orderService, UserManager<IdentityUser> userManager, IFrontService frontService, IWebHostEnvironment webHostEnvironment)
        {
            _orderService = orderService;
            _userManager = userManager;
            _frontService = frontService;
            _webHostEnvironment = webHostEnvironment;
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
        [Authorize, HttpPost]
        public IActionResult AddCartItem(ListItemShopIndexVm result)
        {
            var userId = _userManager.GetUserId(User);
            
            if (result.searchItem is null)
            {
                result.searchItem = string.Empty;
            }          
            _orderService.AddToCart(result.cartItem);
            //var backResult = _frontService.GetItemsByCategory(result.selectedCategory, result.pageSize, result.currentPage, result.searchItem, result.selectedTag, userId);
            //var receivedCategories = _frontService.GetAllCategories(userId);
            //backResult.newsItems = _frontService.GetItemsToIndex(16, "Nowość", userId);
            //backResult.categories = receivedCategories.categories.ToList();
            return RedirectToAction("Index");
            // zrobić cenę netto
        }
        [Authorize, HttpPost]
        public IActionResult AddCartItemAjax(int fItemId, int quantity)
        {
            var iduser = _userManager.GetUserId(User);

            var result = _orderService.AddFavoriteItemToCart(fItemId, quantity, iduser);
            if (result == 1)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }
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
            var userId = _userManager.GetUserId(User);
            var userDetail = _orderService.GetUserDetailById(userId);
            if(userDetail != null)
            {
                _orderForList.userDetail.IsActive = userDetail.IsActive;
                _orderForList.userDetail.IsBlocked= userDetail.IsBlocked;
                _orderForList.userDetail.IsClientBusiness= userDetail.IsClientBusiness;
                _orderForList.userDetail.PriceLevel=userDetail.PriceLevel;
                if (userDetail.IsActive == true)
                {
                    var newOrderId = _orderService.AddOrder(_orderForList);
                    var orderFinished = _orderService.FinishOrder(_orderForList, newOrderId);
                    return RedirectToAction("Index");
                }
            }                   
            string message = "Konto nieaktywne, prosimy o kontakt z obsługą sklepu.";
            TempData["messageError"] = message;
           return RedirectToAction("ErrorPage");          
        }
        [HttpGet,Authorize]
        public IActionResult listOrderForUser()
        {
            var userId = _userManager.GetUserId(User);
            var userOrder = _orderService.GetOrdersByUserId(userId);
            return View(userOrder);
        }
        // Do zrobienia akcja z pobieraniem pdf nazwa akcji "GetPdfDocument"
        [HttpPost,Authorize]
        public IActionResult GetPdfDocument(int orderId)
        {
            var orderPdf = _orderService.GetPdfDocumentFromService(orderId);
            return File(orderPdf, "application/pdf", $"zamowienie_{orderId}.pdf");
        }
        public IActionResult ErrorPage(string message)
        {
            ErrorPageVm errorMessage = new ErrorPageVm();
            errorMessage.errorMessage = TempData["messageError"].ToString();
            errorMessage.imageUrl = $"\\media\\error.png";

            return View(errorMessage);
        }
    }

}
