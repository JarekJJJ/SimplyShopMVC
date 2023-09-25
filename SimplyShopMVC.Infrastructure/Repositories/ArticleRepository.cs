using SimplyShopMVC.Domain.Interface;
using SimplyShopMVC.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Infrastructure.Repositories
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly Context _context;
        public ArticleRepository(Context context)
        {
            _context = context;
        }
        public int AddArticle(Article article)
        {
            _context.Articles.Add(article);
            _context.SaveChanges();
            return article.Id;
        }

        public void DeleteArticle(int articleid)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Article> GetAllArticles()
        {
           var articles= _context.Articles;
            return articles;
        }

        public Article GetArticleById(int articleId)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Article> GetArticlesByTagId(int tagId)
        {
            throw new NotImplementedException();
        }

        public int UpdateArticle(Article article)
        {
            throw new NotImplementedException();
        }
    }
}
