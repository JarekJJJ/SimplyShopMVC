﻿using Microsoft.AspNetCore.Mvc;
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
        [HttpGet]
        public IActionResult Index(int? selectedCategory)
        {

            var listCategories = new ListItemShopIndexVm();         
            if (selectedCategory != null && selectedCategory > 0)
            {
                listCategories = _frontService.GetItemsByCategory((int)selectedCategory, 10, 1, "",0);
                var receivedCategories = _frontService.GetAllCategories();
                listCategories.categories = receivedCategories.categories.ToList();
            }
            else
            {
                listCategories = _frontService.GetAllCategories();
                listCategories.categoryItems = new List<FrontItemForList>();
            }
            listCategories.itemToOrder = new FrontItemForList();
            return View(listCategories);
        }
        [HttpPost]
        public IActionResult Index(ListItemShopIndexVm result, int pageSize, int? pageNo, string searchItem, int? selectedCategory, int? selectedView, int selectedTag)
      // zrobić paginacje z aktywnym filtrem, oraz czyszczenie filtrów
        {
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
                listCategories = _frontService.GetItemsByCategory((int)selectedCategory, pageSize, pageNo.Value, searchItem, selectedTag);
                var receivedCategories = _frontService.GetAllCategories();
                listCategories.categories = receivedCategories.categories.ToList();
            }
            else
            {
                listCategories = _frontService.GetAllCategories();
                listCategories.categoryItems = new List<FrontItemForList>();
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
            var item = _frontService.GetItemDetail(selectedItem);
            return View(item);
        }
        [HttpPost]
        public IActionResult DetailItem(FrontItemForList result)
        {
            var selectedItem = result.id;
            var item = _frontService.GetItemDetail(selectedItem);
            return View(item);
        }
    }
}
