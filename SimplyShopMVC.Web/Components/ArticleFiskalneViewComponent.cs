using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SimplyShopMVC.Application.Interfaces;
using SimplyShopMVC.Application.ViewModels.Article;
using SimplyShopMVC.Application.ViewModels.Front;

namespace SimplyShopMVC.Web.Components
{
    public class ArticleFiskalneViewComponent: ViewComponent //przerobiony na uniwersalny wyświetlacz artykułu !!!
    {
        private readonly IArticleService _articleService; 
        public ArticleFiskalneViewComponent(IArticleService articleService)
        {
            _articleService = articleService;
        }
        public async Task<IViewComponentResult> InvokeAsync(string tagName, int numberArticle, int priority)
        {
            var listArticles = new ListArticleForListVm();
            listArticles = _articleService.GetAllArticleForList(tagName);
           var newListArticles =  listArticles.Articles.OrderByDescending(a=>a.Created).Where(p=>p.Priority==priority).Take(numberArticle).ToList();
            listArticles.Articles= newListArticles;
            //logika podobna do kontrolera          
            return View(listArticles);
        }
    }
}
