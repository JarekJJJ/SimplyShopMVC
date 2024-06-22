using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SimplyShopMVC.Application.ViewModels.Front;
using SimplyShopMVC.Domain.Interface;

namespace SimplyShopMVC.Web.Components
{
    public class BoxNewItemsViewComponent: ViewComponent
    {
        private readonly IFrontService _frontService;
        private readonly UserManager<IdentityUser> _userManager;
        public BoxNewItemsViewComponent(IFrontService frontService, UserManager<IdentityUser> userManager)
        {
            _frontService = frontService;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(string number)
        {
            var listCategories = new ListItemShopIndexVm();
            int intNumber;
            int.TryParse(number,out intNumber);
            var userId = _userManager.GetUserId((System.Security.Claims.ClaimsPrincipal)User);
            var newsItems = _frontService.GetItemsToIndex(intNumber, "Nowość", userId);
            if (userId != null)
            {
                var actualCart = _frontService.GetCart(userId);
                listCategories.newsItems = newsItems;
                listCategories.cart = actualCart;
            }
            //logika podobna do kontrolera          
            return View(listCategories);
        }
    }
}
