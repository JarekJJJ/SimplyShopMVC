using Microsoft.AspNetCore.Mvc;
using SimplyShopMVC.Domain.Interface;
using SimplyShopMVC.Web.Models;
using SimplyShopMVC.Application.Interfaces;
using System.Diagnostics;
using SimplyShopMVC.Application.ViewModels.Item;
using SimplyShopMVC.Application.ViewModels.Front;
using SimplyShopMVC.Application.Services;

namespace SimplyShopMVC.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IFrontService _frontService;

        public HomeController(ILogger<HomeController> logger, IFrontService frontService)
        {
            _logger = logger;
            _frontService = frontService;
        }

        public IActionResult Index()
        {
            IndexListVm listItems= new IndexListVm();
            var items = _frontService.GetItemsToIndex(8, ("Polecamy"));
           listItems.frontItemForLists = items;
            var itemsNews = _frontService.GetItemsToIndex(8, ("Nowość"));
            listItems.frontItemNews = itemsNews;
            return View(listItems);

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}