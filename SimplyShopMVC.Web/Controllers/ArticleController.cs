using Microsoft.AspNetCore.Mvc;
using SimplyShopMVC.Application.Interfaces;

namespace SimplyShopMVC.Web.Controllers
{
    public class ArticleController : Controller
    {
        private readonly IArticleService _articleService;
        public ArticleController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        public IActionResult Index()
        {
            var articles = _articleService.GetAllArticleForList();
            return View(articles);
        }
    }
}
