using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using SimplyShopMVC.Application.ViewModels.Article;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Application.Interfaces
{
    public interface IArticleService
    {
        ListArticleForListVm GetAllArticleForList();
        ArticleDetailVm GetArticleDetails(int articleId);
        UpdateArticleVm GetArticleToUpdate(int articleId);
        int AddArticle(NewArticleVm article, [FromServices] IHostingEnvironment oHostingEnvironment);
        void UpdateArticle(UpdateArticleVm article, [FromServices] IHostingEnvironment oHostingEnvironment);
        void DeleteArticle(int id);
    }
}
