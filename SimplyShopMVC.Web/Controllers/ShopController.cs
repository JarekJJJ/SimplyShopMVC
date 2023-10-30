using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimplyShopMVC.Application.Interfaces;
using SimplyShopMVC.Application.ViewModels.Article;
using SimplyShopMVC.Application.ViewModels.Item;
using System.Data;

namespace SimplyShopMVC.Web.Controllers
{
    public class ShopController : Controller
    {
        private readonly IItemService _itemService;
        private readonly IValidator<AddItemVm> _itemValidator;
        //private readonly IValidator<UpdateArticleVm> _updateArticleValidator;
        public ShopController(IValidator<AddItemVm> itemValidator, IItemService itemService)
        {
            _itemService = itemService;
            _itemValidator = itemValidator;
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
            if(model.TagName!=null)
            {
                var id = _itemService.AddItemTag(model);
                AddItemVm vm = new AddItemVm();
               var newItemPost =  _itemService.AddItem(vm, webHostFolder);
                return RedirectToAction ("AddItem");
            }
            if(model.Category.Name!=null)
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
            _itemService.AddItemWarehouse(model);
            return RedirectToAction("AddItemWarehouse");
        }
        public IActionResult ListItemWarehouse(string searchItem)
        {
            var newItemW = _itemService.ListItemToUpdate(searchItem);
            return View(newItemW);
        }
          
        [HttpGet]
        public IActionResult AdminListItem(string searchItem)
        {
           
            var newItemW = _itemService.ListItemToUpdate(searchItem);
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
            ValidationResult result =  _itemValidator.Validate(model);
            if (result.IsValid &&model.TagName == null && model.Category==null)
            {
                var id = _itemService.UpdateItem(model);
                return RedirectToAction("AdminListItem");
            }
            if (model.TagName != null)
            {
                var id = _itemService.AddItemTag(model);            
                return RedirectToAction("ItemUpdate",new {selectedItem=model.Id});
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

    }
}
