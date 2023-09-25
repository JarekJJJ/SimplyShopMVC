using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using SimplyShopMVC.Application.Interfaces;
using SimplyShopMVC.Application.ViewModels.Article;

namespace SimplyShopMVC.Web.Controllers
{
    public class ArticleController : Controller
    {
        private readonly IArticleService _articleService;
        private readonly IValidator<NewArticleVm> _articleValidator;
        public ArticleController(IArticleService articleService, 
            IValidator<NewArticleVm> articleValidator)
        {
            _articleService = articleService;
            _articleValidator = articleValidator;
        }

        public IActionResult Index()
        {
            var articles = _articleService.GetAllArticleForList();
            return View(articles);
        }
        [HttpGet]
        public IActionResult AddArticle()
        {
            return View(new NewArticleVm());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddArticle(NewArticleVm model)
        {
            ValidationResult result = _articleValidator.Validate(model);
            if (!result.IsValid)
            {
                result.AddToModelState(this.ModelState);
                return View("AddArticle", model);
            }
            if(ModelState.IsValid)
            {
                var id = _articleService.AddArticle(model);
                return RedirectToAction("Index");
            }
            return View("AddArticle", model);

        }
        [HttpGet]
        public IActionResult UpdateArticle(int id)
        {
            var article = _articleService.GetArticleToUpdate(id);
            return View(article);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateArticle(NewArticleVm model)
        {
            ValidationResult result = _articleValidator.Validate(model);
            if (!result.IsValid)
            {
                result.AddToModelState(this.ModelState);
                return View("UpdateArticle", model);
            }
            if (ModelState.IsValid)
            {
                _articleService.UpdateArticle(model);
                return RedirectToAction("Index");
            }
            return View("UpdateArticle", model);

        }
    }
}
