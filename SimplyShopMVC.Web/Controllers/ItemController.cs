using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<IdentityUser> _userManager;

        public ItemController(ILogger<ItemController> logger, IFrontService frontService, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _frontService = frontService;
            _userManager = userManager;
        }
        [Authorize]
        [HttpGet]
        public IActionResult Index(int? selectedCategory)
        {
            var iduser = _userManager.GetUserId(User);
            var listCategories = new ListItemShopIndexVm();         
            if (selectedCategory != null && selectedCategory > 0)
            {
                listCategories = _frontService.GetItemsByCategory((int)selectedCategory, 10, 1, "",0, iduser);
                var receivedCategories = _frontService.GetAllCategories(iduser);
                listCategories.categories = receivedCategories.categories.ToList();
            }
            else
            {
                listCategories = _frontService.GetAllCategories(iduser);
                listCategories.categoryItems = new List<FrontItemForList>();
                var newsItems = _frontService.GetItemsToIndex(16, "Nowość", iduser);
                listCategories.newsItems = newsItems;
            }
            listCategories.itemToOrder = new FrontItemForList();
            return View(listCategories);
        }
        [HttpPost]
        public IActionResult Index(ListItemShopIndexVm result, int pageSize, int? pageNo, string searchItem, int? selectedCategory, int? selectedView, int selectedTag)
      // zrobić paginacje z aktywnym filtrem, oraz czyszczenie filtrów
        {
            var iduser = _userManager.GetUserId(User);
            if (!pageNo.HasValue)
            {
                pageNo = 1;
            }
            if(searchItem is null)
            {
                searchItem = string.Empty;
            }
            if(pageSize==0)
            {
                pageSize = 10;
            }
            var listCategories = new ListItemShopIndexVm();
            
            if (selectedCategory != null && selectedCategory > 0)
            {
                listCategories = _frontService.GetItemsByCategory((int)selectedCategory, pageSize, pageNo.Value, searchItem, selectedTag, iduser);
                var receivedCategories = _frontService.GetAllCategories(iduser);
                listCategories.categories = receivedCategories.categories.ToList();
            }
            else
            {
                listCategories = _frontService.GetAllCategories(iduser);
                listCategories.categoryItems = new List<FrontItemForList>();
                var newsItems = _frontService.GetItemsToIndex(16, "Nowość", iduser);
                listCategories.newsItems = newsItems;
            }
            if (selectedView > 0)
            {
                listCategories.selectedView = 1;
            }
            return View(listCategories);
        }
        [HttpGet]
        public IActionResult DetailItem(int selectedItem)
        {
            var iduser = _userManager.GetUserId(User);
            var item = _frontService.GetItemDetail(selectedItem, iduser);
            return View(item);
        }
        [HttpPost]
        public IActionResult DetailItem(FrontItemForList result)
        {
            var iduser = _userManager.GetUserId(User);
            var selectedItem = result.id;
            var item = _frontService.GetItemDetail(selectedItem, iduser);
            return View(item);
        }
    }
}
