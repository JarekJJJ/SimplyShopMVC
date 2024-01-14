using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimplyShopMVC.Application.Interfaces;
using SimplyShopMVC.Application.ViewModels.Article;
using SimplyShopMVC.Application.ViewModels.Item;
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
        //private readonly IValidator<UpdateArticleVm> _updateArticleValidator;
        public ShopController(IValidator<AddItemVm> itemValidator, IItemService itemService, ISettingsService settingsService)
        {
            _itemService = itemService;
            _itemValidator = itemValidator;
            _settingsService = settingsService;
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
           var result =  _itemService.AddItemWarehouse(model);
            return View(result);
        }
        public IActionResult ListItemWarehouse(string searchItem)
        {
            var newItemW = _itemService.ListItemToUpdate(searchItem,0);
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
    }
}
