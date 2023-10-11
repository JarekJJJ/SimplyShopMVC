using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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

        public int AddArticleTag(ArticleTag articleTag)
        {
            _context.ArticleTags.Add(articleTag);
            _context.SaveChanges();
            return articleTag.Id;
        }

        public void AddConnectionArticleTags(int articleId, ArticleTag tag)
        {
            var article = GetArticleById(articleId);
            ConnectArticleTag con = new ConnectArticleTag();
            con.ArticleId = article.Id;
            con.ArticleTagId = tag.Id;
            _context.ConnectArticleTag.Add(con);
            _context.SaveChanges();
        }

        public void DeleteArticle(int articleid)
        {
            var article = _context.Articles.Find(articleid);
            _context.Articles.Remove(article);
            _context.SaveChanges();
        }

        public void DeleteConnectionArticleTags(int articleId)
        {
            var result = _context.ConnectArticleTag.Where(a => a.ArticleId == articleId);
            _context.ConnectArticleTag.RemoveRange(result);
            _context.SaveChanges();
        }

        public IQueryable<Article> GetAllArticles()
        {
            var articles = _context.Articles;
            return articles;
        }

        public IQueryable<ArticleTag> GetAllArticleTags()
        {
            var tags = _context.ArticleTags;
            return tags;
        }

        public Article GetArticleById(int articleId)
        {

            var result = _context.Articles.Where(a => a.Id == articleId).Include(a => a.ConnectArticleTags).ThenInclude(at => at.ArticleTag).FirstOrDefault();
                //FirstOrDefault(a => a.Id == articleId);
            return result;
        }

        public IQueryable<Article> GetArticlesByTagId(int tagId)
        {
            var result = _context.Articles.Where(at => at.ConnectArticleTags.Any(at=>at.ArticleTagId==tagId));           
            return result;
        }

        public ArticleTag GetArticleTagByTagId(int tagId)
        {
            var result = _context.ArticleTags.FirstOrDefault(t => t.Id == tagId);
            return result;
        }

        public IQueryable<ConnectArticleTag> GetConnectArticleTags(int articleId)
        {
           var result = _context.ConnectArticleTag.Where(t=>t.ArticleId== articleId);
            return result;
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
