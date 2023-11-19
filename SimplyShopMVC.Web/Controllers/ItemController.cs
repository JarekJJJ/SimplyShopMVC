using Microsoft.AspNetCore.Mvc;
using SimplyShopMVC.Application.Services;
using SimplyShopMVC.Application.ViewModels.Front;
using SimplyShopMVC.Domain.Interface;

namespace SimplyShopMVC.Web.Controllers
{
    public class ItemController : Controller
    {
        private readonly ILogger<ItemController> _logger;
        private readonly IFrontService _frontService;

        public ItemController(ILogger<ItemController> logger, IFrontService frontService)
        {
            _logger = logger;
            _frontService = frontService;
        }
        public IActionResult Index(int? selectedCategory)
        {

            var listCategories = new ListItemShopIndexVm();
            if (selectedCategory != null && selectedCategory > 0)
            {
                listCategories = _frontService.GetItemsByCategory((int)selectedCategory);
                var receivedCategories = _frontService.GetAllCategories();
                listCategories.categories = receivedCategories.categories.ToList();
            }
            else
            {
               listCategories = _frontService.GetAllCategories();
                listCategories.categoryItems = new List<FrontItemForList>();
            }
            
            return View(listCategories);
        }
        public IActionResult DetailItem(int selectedItem)
        {
            return View();
        }
    }
}
