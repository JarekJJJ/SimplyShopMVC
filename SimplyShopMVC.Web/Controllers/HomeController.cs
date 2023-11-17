using Microsoft.AspNetCore.Mvc;
using SimplyShopMVC.Domain.Interface;
using SimplyShopMVC.Web.Models;
using SimplyShopMVC.Application.Interfaces;
using System.Diagnostics;

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
            var items = _frontService.GetItemsToIndex(0,8);
           
            return View(items);
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