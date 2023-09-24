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
        void DeleteArticle(int Articleid);

        int AddArticle(Article Article);
        int UpdateArticle(Article Article);

        IQueryable<Article> GetArticlesByTagId(int tagId);

        Article GetArticleById(int articleId);

        IQueryable<Article> GetAllArticles();
    }
}
