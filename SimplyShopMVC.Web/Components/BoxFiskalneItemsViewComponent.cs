using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SimplyShopMVC.Application.ViewModels.Front;
using SimplyShopMVC.Domain.Interface;

namespace SimplyShopMVC.Web.Components
{
    public class BoxFiskalneItemsViewComponent: ViewComponent
    {
        private readonly IFrontService _frontService;
        private readonly UserManager<IdentityUser> _userManager;
        public BoxFiskalneItemsViewComponent(IFrontService frontService, UserManager<IdentityUser> userManager)
        {
            _frontService = frontService;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(string number)
        {
            var listCategories = new ListItemShopIndexVm();
            int intNumber;
            int.TryParse(number, out intNumber);
            var userId = _userManager.GetUserId((System.Security.Claims.ClaimsPrincipal)User);
            var newsItems = _frontService.GetItemsToIndex(intNumber, "uFiskalne", userId);
            if (userId != null)
            {
                var actualCart = _frontService.GetCart(userId);
                listCategories.cart = actualCart;
            }
            listCategories.newsItems = newsItems;
            //logika podobna do kontrolera          
            return View(listCategories);
        }
    }
}

