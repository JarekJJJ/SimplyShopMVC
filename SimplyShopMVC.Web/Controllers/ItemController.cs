using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using SimplyShopMVC.Application.Interfaces;
using SimplyShopMVC.Application.Services;
using SimplyShopMVC.Application.ViewModels.Front;
using SimplyShopMVC.Application.ViewModels.Order;
using SimplyShopMVC.Application.ViewModels.PcSets;
using SimplyShopMVC.Domain.Interface;
using Newtonsoft.Json;

namespace SimplyShopMVC.Web.Controllers
{
    public class ItemController : Controller
    {
        private readonly ILogger<ItemController> _logger;
        private readonly IFrontService _frontService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ISettingsService _settingsService;
        private readonly ISetService _setService;

        public ItemController(ILogger<ItemController> logger, IFrontService frontService, UserManager<IdentityUser> userManager, ISettingsService settingsService, ISetService setService)
        {
            _logger = logger;
            _frontService = frontService;
            _userManager = userManager;
            _settingsService = settingsService;
            _setService = setService;
        }
        
        [HttpGet]
        public IActionResult Index(int? selectedCategory, string searchString)
        {
            var listCategories = new ListItemShopIndexVm();
            var iduser = _userManager.GetUserId(User);
            var userDetail = _userManager.GetUserName(User);
            if(iduser != null)
            {
                _settingsService.AddUserSettings(iduser, userDetail);
            }
            if (String.IsNullOrEmpty(searchString))
            {
                searchString= string.Empty;
            }
            if(selectedCategory == null)
            {
                selectedCategory = 0;
            }
            if (( selectedCategory > 0) || (!String.IsNullOrEmpty(searchString)))
            {
                listCategories = _frontService.GetItemsByCategory((int)selectedCategory, 15, 1, searchString, 0, iduser);
                var receivedCategories = _frontService.GetAllCategories(iduser);
                listCategories.categories = receivedCategories.categories.ToList();
            }
            else
            {
                listCategories = _frontService.GetAllCategories(iduser);
                listCategories.categoryItems = new List<FrontItemForList>();
               // var newsItems = _frontService.GetItemsToIndex(16, "Nowość", iduser);
               // listCategories.newsItems = newsItems;
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
            if (searchItem is null)
            {
                searchItem = string.Empty;
            }
            if (pageSize == 0)
            {
                pageSize = 10;
            }
            if (selectedCategory == null)
            {
                selectedCategory = 0;
            }
            var listCategories = new ListItemShopIndexVm();

            if ( (selectedCategory > 0) || (!String.IsNullOrEmpty(searchItem)))
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
            ViewBag.JsonData = JsonConvert.SerializeObject(item.omnibusPriceList);
            var fullUrl = Url.Action("DetailItem", "Item", new { selectedItem = item.id }, Request.Scheme);
            ViewBag.FullUrl = fullUrl;
            ViewBag.ImgUrl = Request.Scheme + "://" + Request.Host.Host;
            return View(item);
        }
        //[HttpPost]
        //public IActionResult DetailItem(FrontItemForList result)
        //{
        //    var iduser = _userManager.GetUserId(User);
        //    var selectedItem = result.id;
        //    var item = _frontService.GetItemDetail(selectedItem, iduser);
        //    return View(item);
        //}
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult AddPcSet(ListItemShopIndexVm result, int options)
        {
            _setService.SetHandling(result, options);
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult AdminViewPcSet()
        {
            ListItemShopIndexVm result = new ListItemShopIndexVm();
            var listPcSet = _setService.SetHandling(result, 0);
            return View(listPcSet);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult AdminEditPcSet(ListPcSetsForListVm resultSet, int options)
        {
            ListItemShopIndexVm result = new ListItemShopIndexVm();
            result.pcSets = new PcSetsForListVm();
            if (resultSet.pcSet == null)
            {
                var idPc = TempData["pcsetId"]; //Dane z post
                if (idPc != null)
                {
                    result.pcSets.Id = (int)idPc;
                }
            }
            else
            {
                result.pcSets.Id = resultSet.pcSet.Id;
            }
            var listPcSet = _setService.SetHandling(result, options);
            return View(listPcSet);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult AdminEditPcSet(ListPcSetsForListVm resultSet, int options, int back)
        {
            ListItemShopIndexVm result = new ListItemShopIndexVm();
            result.pcSets = new PcSetsForListVm();
            result.pcSets = resultSet.pcSet;
            result.setItem = resultSet.setItem;
            result.selectedImage = resultSet.selectedImg;
            TempData["pcsetId"] = resultSet.pcSet.Id;
            result.Image = resultSet.Image;
            var listPcSet = _setService.SetHandling(result, options);
            return RedirectToAction("AdminEditPcSet");
        }
        [HttpGet]
        public IActionResult PcSetViewForUser()
        {
            ListPcSetsForListVm pcSetView = new ListPcSetsForListVm();
            var iduser = _userManager.GetUserId(User);
            pcSetView = _setService.ListSetForUser(iduser);
            return View(pcSetView);
        }
        [HttpGet]
        public IActionResult PcSetViewDetail(int pcSetId)
        {
            PcSetDetailVm pcSetDetail= new PcSetDetailVm();
            var iduser = _userManager.GetUserId(User);
            pcSetDetail = _setService.PcSetDetailForUser(pcSetId, iduser);
            return View(pcSetDetail);
        }
        [Authorize]
        [HttpGet]
        public IActionResult FavoriteItemList()
        {
            var iduser = _userManager.GetUserId(User);
            var favoriteItemList = _frontService.GetAllFavoriteItems(iduser);
            return View(favoriteItemList);
        }
        [Authorize, HttpPost]
        public IActionResult AddFavoriteItemToList(int fItemId)
        {
            var iduser = _userManager.GetUserId(User);

            var result = _frontService.AddFavoriteItemToList(fItemId, iduser);
            if(result == 1)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }
        }
        [ValidateAntiForgeryToken, Authorize, HttpPost]
        public IActionResult DeleteFavoriteItem(int favoriteItemId)
        {
            _frontService.DeleteFavoriteItemFromList(favoriteItemId);
            TempData["success"] = "Usunięto pozycje z listy ulubionych produktów";
            return RedirectToAction("FavoriteItemList");
        }
    }
}
