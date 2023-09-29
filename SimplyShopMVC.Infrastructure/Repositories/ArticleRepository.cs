using Microsoft.AspNetCore.Identity;
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
            var article = _context.Articles.Find(articleid);
            _context.Articles.Remove(article);
            _context.SaveChanges();
        }

        public IQueryable<Article> GetAllArticles()
        {
            var articles = _context.Articles;
            return articles;
        }

        public Article GetArticleById(int articleId)
        {
            var result = _context.Articles.FirstOrDefault(a => a.Id == articleId);
            return result;
        }

        public IQueryable<Article> GetArticlesByTagId(int tagId)
        {
            throw new NotImplementedException();
        }

        public void UpdateArticle(Article article)
        {
            //var _article = _context.Articles.Where(i=>i.Id == article.Id).FirstOrDefault();
            //if (_article != null)
            //{
            //    _article = article;
            //    _context.SaveChanges();
            //}
            _context.Articles.Update(article);
            _context.SaveChanges();
        }      
    }
}
