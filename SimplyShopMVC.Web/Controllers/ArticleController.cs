using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimplyShopMVC.Application.Interfaces;
using SimplyShopMVC.Application.ViewModels.Article;
using SimplyShopMVC.Web.Filters;

namespace SimplyShopMVC.Web.Controllers
{
    
    public class ArticleController : Controller
    {
        private readonly IArticleService _articleService;
        private readonly IValidator<NewArticleVm> _articleValidator;
        private readonly IValidator<UpdateArticleVm> _updateArticleValidator;
        public ArticleController(IArticleService articleService, 
            IValidator<NewArticleVm> articleValidator, IValidator<UpdateArticleVm> updateArticleValidator)
        {
            _articleService = articleService;
            _articleValidator = articleValidator;
            _updateArticleValidator= updateArticleValidator;
        }
       // [Authorize]
       // [CheckPermissions("Read")]
        public IActionResult Index()
        {
            var articles = _articleService.GetAllArticleForList();
            return View(articles);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult AddArticle()
        {
            return View(new NewArticleVm());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddArticle(NewArticleVm model, [FromServices] Microsoft.AspNetCore.Hosting.IHostingEnvironment oHostingEnvironment)
        {
            ValidationResult result = await _articleValidator.ValidateAsync(model);
            if (!result.IsValid)
            {
                result.AddToModelState(this.ModelState);
                return View("AddArticle", model);
            }
            if(ModelState.IsValid)
            {
                var id = _articleService.AddArticle(model, oHostingEnvironment);               
                return RedirectToAction("Index");
            }
            return View("AddArticle", model);

        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult UpdateArticle(int id)
        {
            var article = _articleService.GetArticleToUpdate(id);
            return View(article);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateArticle(UpdateArticleVm model, [FromServices] Microsoft.AspNetCore.Hosting.IHostingEnvironment oHostingEnvironment, List<string> SelectedImage)
        {
            ValidationResult result = _updateArticleValidator.Validate(model);
            if (!result.IsValid)
            {
                result.AddToModelState(this.ModelState);
                return View("UpdateArticle", model);
            }
            if (result.IsValid)
            {
                _articleService.UpdateArticle(model, oHostingEnvironment, SelectedImage);
                return RedirectToAction("Index");
            }
            return View("UpdateArticle", model);

        }
        public IActionResult Delete(int id)
        {
            _articleService.DeleteArticle(id);
            return RedirectToAction("Index");
        }
        public IActionResult ArticleDetail(int id)
        {
            var articleDetail = _articleService.GetArticleDetails(id);
            return View(articleDetail);
        }
    }
}
