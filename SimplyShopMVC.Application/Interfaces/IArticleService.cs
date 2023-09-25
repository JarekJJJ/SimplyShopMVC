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
        NewArticleVm GetArticleToUpdate(int articleId);
        int AddArticle(NewArticleVm article);
        void UpdateArticle(NewArticleVm article);
        int DeleteArticle(int id);
        


    }
}
