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
        public ShopController(IItemService itemService)
        {
            _itemService = itemService;
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
           if(ModelState.IsValid)
            {
                var id = _itemService.AddItem(model, webHostFolder);
                return RedirectToAction("Index");
            }
            if(model.TagName!=null)
            {
                var id = _itemService.AddItemTag(model);
                AddItemVm vm = new AddItemVm();
               model =  _itemService.AddItem(vm, webHostFolder);
                return View ("AddItem", model);
            }
            if(model.categoryName!=null)
            {
                var id = _itemService.AddCategory(model);
                return View("AddItem", model);
            }
            return View("AddItem", model);

        }
    }
}
