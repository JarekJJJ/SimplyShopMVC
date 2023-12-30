using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SimplyShopMVC.Application.Interfaces;
using SimplyShopMVC.Domain.Interface;

namespace SimplyShopMVC.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly UserManager<IdentityUser> _userManager;
        public CartController(IOrderService orderService, UserManager<IdentityUser> userManager)
        {
            _orderService = orderService;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var userId = _userManager.GetUserId(User);
            var cartWithItems = _orderService.GetCartWithCartItems(userId);
            return PartialView("_CartPartial", cartWithItems);
       // return View(cartWithItems);
        }
    }
}
