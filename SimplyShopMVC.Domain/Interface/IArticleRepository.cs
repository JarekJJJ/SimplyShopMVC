using SimplyShopMVC.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Domain.Interface
{
    public interface IArticleRepository
    {
        void DeleteArticle(int articleid);

        int AddArticle(Article article);
        void UpdateArticle(Article article);

        IQueryable<Article> GetArticlesByTagId(int tagId);

        Article GetArticleById(int articleId);

        IQueryable<Article> GetAllArticles();
    }
}
