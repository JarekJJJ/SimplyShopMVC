using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimplyShopMVC.Application.Interfaces;
using SimplyShopMVC.Application.Services;
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
            _updateArticleValidator = updateArticleValidator;
        }
        public IActionResult Index()
        {
            string tag = TempData["tagName"] as string;
            if (String.IsNullOrEmpty(tag))
            {
                var articles = _articleService.GetAllArticleForList();
                return View(articles);
            }
            else
            {
                var articles = _articleService.GetAllArticleForList(tag);
                return View(articles);
            }
        }
        [HttpGet]
        public IActionResult GetInfoArticle()
        {
            TempData["tagName"] = "Informacje";
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult GetFiskalneArticle()
        {
            TempData["tagName"] = "Fiskalne";
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult GetGamingArticle()
        {
            TempData["tagName"] = "GamingPC";
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult AddArticle([FromServices] Microsoft.AspNetCore.Hosting.IHostingEnvironment oHostingEnvironment)
        {
            NewArticleVm vm = new NewArticleVm();
            var newArticle = _articleService.AddArticle(vm, oHostingEnvironment);
            return View(newArticle);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddArticle(NewArticleVm model, [FromServices] Microsoft.AspNetCore.Hosting.IHostingEnvironment oHostingEnvironment)
        {
            if (model.Title != null)
            {
                ValidationResult result = await _articleValidator.ValidateAsync(model);
                if (!result.IsValid)
                {
                    result.AddToModelState(this.ModelState);
                    return View("AddArticle", model);
                }
                if (result.IsValid)
                {
                    var id = _articleService.AddArticle(model, oHostingEnvironment);
                    return RedirectToAction("Index");
                }
            }
            else
            {
                var id = _articleService.AddTag(model);
                NewArticleVm vm = new NewArticleVm();
                model = _articleService.AddArticle(vm, oHostingEnvironment);
            }
            return View("AddArticle", model);

        }
        [HttpGet, Authorize(Roles = "Admin")]
        public IActionResult AdminListArticle()
        {
            var articles = _articleService.GetAllArticleForList();
            return View(articles);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult UpdateArticle(int id)
        {
            var article = _articleService.GetArticleToUpdate(id);
            return View(article);
        }
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            _articleService.DeleteArticle(id);
            return RedirectToAction("AdminListArticle");
        }
        public IActionResult ArticleDetail(int id)
        {
            var articleDetail = _articleService.GetArticleDetails(id);
            return View(articleDetail);
        }
        public IActionResult ListArticle(int idTag)
        {
            var articles = _articleService.GetAllArticlesByTagId(idTag);

            return View("ListArticle", articles);
        }
        public IActionResult ListArticleTagToUpdate(string? searchTag)
        {
            var articleTags = _articleService.ListArticleTagToUpdate(searchTag);
            return View(articleTags);
        }
        [HttpGet]
        public IActionResult UpdateArticleTag(int articleTagId)
        {
            var articleTag = _articleService.GetArticleTagToUpdate(articleTagId);
            return View(articleTag);
        }
        [HttpPost]
        public IActionResult UpdateArticleTag(UpdateArticleTagVm articleTag, int options)
        {
            _articleService.UpdateArticleTag(articleTag, options);
            return RedirectToAction("ListArticleTagToUpdate");
        }
    }
}

