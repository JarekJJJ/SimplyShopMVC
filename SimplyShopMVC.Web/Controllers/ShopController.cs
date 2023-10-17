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
            if(model.categoryName!=null)
            {
                var id = _itemService.AddCategory(model);
                AddItemVm vm = new AddItemVm();
                var newItemPost = _itemService.AddItem(vm, webHostFolder);
                return RedirectToAction("AddItem");
            }
            
            return RedirectToAction("AddItem");

        }
    }
}
