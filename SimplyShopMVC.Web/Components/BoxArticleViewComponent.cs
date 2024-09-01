using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SimplyShopMVC.Application.Interfaces;
using SimplyShopMVC.Application.ViewModels.Article;
using SimplyShopMVC.Application.ViewModels.Front;

namespace SimplyShopMVC.Web.Components;
public class BoxArticleViewComponent: ViewComponent
{
    private readonly IArticleService _articleService;
    public BoxArticleViewComponent( IArticleService articleService)
    {
        _articleService= articleService;
    }
    public async Task<IViewComponentResult> InvokeAsync(int number, string tagName)
    {
        var listArticles = new ListArticleForListVm();
        if (!string.IsNullOrEmpty(tagName))
        {   
            listArticles = _articleService.GetAllArticleForList(tagName);
            var newListArticles = listArticles.Articles.OrderByDescending(a => a.Created).Take(number).ToList();
            listArticles.Articles = newListArticles;
        }
        else
        {
            listArticles = _articleService.GetAllArticleForList();
            var newListArticles = listArticles.Articles.OrderByDescending(a => a.Created).Take(number).ToList();
            listArticles.Articles = newListArticles;
        }

        return View(listArticles);
    }
}
