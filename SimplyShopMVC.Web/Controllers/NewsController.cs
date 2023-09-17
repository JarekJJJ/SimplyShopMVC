using Microsoft.AspNetCore.Mvc;

namespace SimplyShopMVC.Web.Controllers
{
    public class NewsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
