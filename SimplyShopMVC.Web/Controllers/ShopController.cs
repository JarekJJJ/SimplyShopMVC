using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimplyShopMVC.Application.Interfaces;
using SimplyShopMVC.Application.ViewModels.Article;
using SimplyShopMVC.Application.ViewModels.Item;
using SimplyShopMVC.Application.ViewModels.Order;
using SimplyShopMVC.Application.ViewModels.user;
using SimplyShopMVC.Domain.Model;
using System.Data;

namespace SimplyShopMVC.Web.Controllers
{
    public class ShopController : Controller
    {
        private readonly IItemService _itemService;
        private readonly IValidator<AddItemVm> _itemValidator;
        private readonly ISettingsService _settingsService;
        private readonly IOrderService _orderService;
        //private readonly IValidator<UpdateArticleVm> _updateArticleValidator;
        public ShopController(IValidator<AddItemVm> itemValidator, IItemService itemService, ISettingsService settingsService, IOrderService orderService)
        {
            _itemService = itemService;
            _itemValidator = itemValidator;
            _settingsService = settingsService;
            _orderService = orderService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult AddItem([FromServices] IWebHostEnvironment webHostFolder)
        {
            AddItemVm vm = new AddItemVm();
            var newItem = _itemService.AddItem(vm, webHostFolder);

            return View(newItem);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddItem(AddItemVm model, [FromServices] IWebHostEnvironment webHostFolder)
        {
            ValidationResult result = await _itemValidator.ValidateAsync(model);
            if (result.IsValid)
            {
                var id = _itemService.AddItem(model, webHostFolder);
                return RedirectToAction("AddItem");
            }
            if (model.TagName != null)
            {
                var id = _itemService.AddItemTag(model);
                AddItemVm vm = new AddItemVm();
                var newItemPost = _itemService.AddItem(vm, webHostFolder);
                return RedirectToAction("AddItem");
            }
            if (model.Category.Name != null)
            {
                var id = _itemService.AddCategory(model);
                AddItemVm vm = new AddItemVm();
                var newItemPost = _itemService.AddItem(vm, webHostFolder);
                return RedirectToAction("AddItem");
            }

            return RedirectToAction("AddItem");

        }
        [HttpGet]
        public IActionResult AddItemWarehouse(int itemId)
        {
            var listWarehouseItem = _itemService.AddToUpdateItemWarehouse(itemId);
            return View(listWarehouseItem);
        }
        [HttpPost]
        public IActionResult AddItemWarehouse(AddItemWarehouseVm model)
        {
            var result = _itemService.AddItemWarehouse(model);
            return View(result);
        }
        public IActionResult ListItemWarehouse(string searchItem)
        {
            var newItemW = _itemService.ListItemToUpdate(searchItem, 0);
            return View(newItemW);
        }

        [HttpGet]
        public IActionResult AdminListItem(string searchItem, int countItem)
        {

            var newItemW = _itemService.ListItemToUpdate(searchItem, countItem);
            return View(newItemW);
        }
        [HttpPost]
        public IActionResult AdminListItem(AddItemWarehouseVm item)

        {
            _itemService.UpdateItemFromList(item);
            var newItemW = _itemService.ListItemToUpdate("", 0);
            return View(newItemW);
        }
        [HttpGet]
        public IActionResult ItemUpdate(int selectedItem)
        {
            var itemToUpdate = _itemService.AddItemToUpdate(selectedItem);
            return View(itemToUpdate);
        }
        [HttpPost]
        public IActionResult ItemUpdate(AddItemVm model, [FromServices] IWebHostEnvironment webHostFolder)
        {
            ValidationResult result = _itemValidator.Validate(model);
            if (result.IsValid && model.TagName == null && model.Category == null)
            {
                var id = _itemService.UpdateItem(model);
                return RedirectToAction("AdminListItem");
            }
            if (model.TagName != null)
            {
                var id = _itemService.AddItemTag(model);
                return RedirectToAction("ItemUpdate", new { selectedItem = model.Id });
            }
            if (model.Category.Name != null)
            {
                var id = _itemService.AddCategory(model);
                return RedirectToAction("ItemUpdate", new { selectedItem = model.Id });
            }
            return RedirectToAction("AdminListItem");
        }
        public IActionResult ListItemTagToUpdate(string? searchTag)
        {
            var itemTags = _itemService.ListItemTagToUpdate(searchTag);
            return View(itemTags);
        }
        [HttpGet]
        public IActionResult UpdateItemTag(int itemTagId)
        {
            var itemTag = _itemService.GetItemTagToUpdate(itemTagId);
            return View(itemTag);
        }
        [HttpPost]
        public IActionResult UpdateItemTag(UpdateItemTagVm model, int options)
        {
            _itemService.UpdateItemTag(model, options);
            return RedirectToAction("ListItemTagToUpdate");
        }
        // ----------- Category ----------
        public IActionResult ListCategoryToUpdate(string? searchCategory)
        {
            var categoryList = _itemService.ListCategoryToUpdate(searchCategory);
            return View(categoryList);
        }
        [HttpGet]
        public IActionResult UpdateCategory(int categoryId)
        {
            var category = _itemService.GetCategoryToUpdate(categoryId);
            return View(category);
        }
        [HttpPost]
        public IActionResult UpdateCategory(UpdateCategoryVm model, int options)
        {
            _itemService.UpdateCategory(model, options);
            return RedirectToAction("ListCategoryToUpdate");
        }
        //----------Warehouse-----------------
        public IActionResult ListWarehouseToUpdate(string? searchWarehouse)
        {
            var warehouseList = _itemService.ListWarehouseToUpdate(searchWarehouse);
            return View(warehouseList);
        }
        [HttpGet]
        public IActionResult UpdateWarehouse(int warehouseId)
        {
            var warehouse = _itemService.GetWarehouseToUpdate(warehouseId);
            return View(warehouse);
        }
        [HttpPost]
        public IActionResult UpdateWarehouse(UpdateWarehouseVm model, int options)
        {
            _itemService.UpdateWarehouse(model, options);
            return RedirectToAction("ListWarehouseToUpdate");
        }
        // Admin Settings
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult CompanySettings()
        {
            CompanySettingsVm company = new CompanySettingsVm();
            company = _settingsService.EditCompanySettings(0, company);
            return View(company);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult CompanySettings(int flag, CompanySettingsVm company)
        {
            var result = _settingsService.EditCompanySettings(flag, company);
            return View(result);
        }
        //Order
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult GetOrderForAdmin(int? filterOptions, string? searchString)
        {
            if (!filterOptions.HasValue)
            {
                filterOptions = 0;
            }
            if (String.IsNullOrEmpty(searchString))
            {
                searchString = string.Empty;
            }
            var result = _orderService.GetOrdersForAdmin(0, "", 0, (int)filterOptions, searchString);
            return View("AdminListOrder", result);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult GetOrderForAdmin(int status, OrderForAdminListVm order, int? filterOptions, string? searchString)
        {
            if (!filterOptions.HasValue)
            {
                filterOptions = 0;
            }
            if (String.IsNullOrEmpty(searchString))
            {
                searchString = string.Empty;
            }
            var result = _orderService.GetOrdersForAdmin(status, "", order.selectedOrders, (int)filterOptions, searchString);
            return RedirectToAction("GetOrderForAdmin");
        }
        [Authorize(Roles = "Admin"), HttpPost]
        public IActionResult ViewOrderForAdmin(int orderId, string userId)
        {
            var result = _orderService.ViewOrderForAdmin(orderId, userId);
            return View(result);
        }
        [Authorize(Roles = "Admin"), HttpPost]
        public IActionResult adminFinishOrder(OrderForAdminListVm result, int options)
        {           
            _orderService.AdminFinishOrder(result, options);
            return RedirectToAction("GetOrderForAdmin");
        }
        [Authorize(Roles = "Admin"), HttpGet]
        public IActionResult GroupItem()
        {
            GroupItemForListVm groupItem = new GroupItemForListVm();
            var result = _itemService.GroupsItemsList(0, groupItem);
            return View(result);
        }
        [Authorize(Roles = "Admin"), HttpPost]
        public IActionResult GroupItem(ListGroupItemForListVm listGroupItemForList, int options)
        {
           
            _itemService.GroupsItemsList(options, listGroupItemForList.GroupItem);
            return RedirectToAction("GroupItem");
        }
        [Authorize(Roles = "Admin"), HttpGet]
        public IActionResult UserSettings()
        {
            string searchString = string.Empty;
            var userList = _settingsService.UserSettings( new ListUserDetailForListVm(), 0, searchString);
            return View(userList);
        }
        [Authorize(Roles = "Admin"), HttpPost]
        public IActionResult UserSettings(ListUserDetailForListVm result, int options, string searchString)
        {
            if (String.IsNullOrEmpty(searchString))
            {
                searchString= string.Empty;
            }
            if(options >0)
            {
               var userList = _settingsService.UserSettings(result, options, searchString);              
                return View(userList);
            }
            else
            {
                var userList = _settingsService.UserSettings(result, options, searchString);
            }
            
            return RedirectToAction("UserSettings");
        }

    }
}
