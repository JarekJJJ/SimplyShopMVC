using Microsoft.AspNetCore.Mvc;
using SimplyShopMVC.Application.ViewModels.Front;
using SimplyShopMVC.Domain.Interface;

namespace SimplyShopMVC.Web.Components
{
    public class BoxNewItemsViewComponent: ViewComponent
    {
        private readonly IFrontService _frontService;
        public BoxNewItemsViewComponent(IFrontService frontService)
        {
            _frontService = frontService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string number)
        {
            var listCategories = new ListItemShopIndexVm();
            int intNumber;
            int.TryParse(number,out intNumber);
            var newsItems = _frontService.GetItemsToIndex(intNumber, "Nowość", "");
            listCategories.newsItems = newsItems;
            //logika podobna do kontrolera          
            return View(listCategories);
        }
    }
}
