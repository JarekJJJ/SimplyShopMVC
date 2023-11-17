using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult DetailItem(int id)
        {
            return View();
        }
    }
}
