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
        void UpdateArticle(Article article);

        IQueryable<Article> GetArticlesByTagId(int tagId);
        IQueryable<ConnectArticleTag> GetConnectArticleTags(int articleId);

        Article GetArticleById(int articleId);

        IQueryable<Article> GetAllArticles();
        IQueryable<ArticleTag> GetAllArticleTags();
        int AddArticleTag(ArticleTag articleTag);
        ArticleTag GetArticleTagByTagId(int tagId);
        int AddArticle(Article article);
        void AddConnectionArticleTags(int articleId ,ArticleTag tags);
    }
}
