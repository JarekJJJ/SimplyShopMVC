using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Identity;
using SimplyShopMVC.Application.Interfaces;
using SimplyShopMVC.Application.ViewModels.Article;
using SimplyShopMVC.Domain.Interface;
using SimplyShopMVC.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Application.Services
{
    public class ArticleService : IArticleService
    {
        private readonly IArticleRepository _articleRepo;
        private readonly IMapper _mapper;
        public ArticleService(IArticleRepository articleRepo, IMapper mapper)
        {
            _articleRepo = articleRepo;
            _mapper = mapper;
        }

        public int AddArticle(NewArticleVm article)
        {
            var art = _mapper.Map<Article>(article); //Wskazuje docelowy model w domain i przekazuje obiekt article
            var id = _articleRepo.AddArticle(art);
            return id;
        }

        public void DeleteArticle(int id)
        {
            _articleRepo.DeleteArticle(id);
        }

        public ListArticleForListVm GetAllArticleForList()
        {
            //.projectTo - stosujemy przy kolekcjacj Iquerable
            var articles = _articleRepo.GetAllArticles()
                  .ProjectTo<ArticleForListVm>(_mapper.ConfigurationProvider).ToList();
            var articlesList = new ListArticleForListVm()
            {
                Articles = articles,
                Count = articles.Count
            };           
            return articlesList;
        }

        public ArticleDetailVm GetArticleDetails(int articleId)
        {
            // metoda _mapper.Map jest wykorzystywana przy pojedynczym obiekcie!
           var article = _articleRepo.GetArticleById(articleId);
            var articleVm = _mapper.Map<ArticleDetailVm>(article);
            return articleVm;
        }

        public NewArticleVm GetArticleToUpdate(int articleId)
        {
            var article = _articleRepo.GetArticleById(articleId);
            var articleVm = _mapper.Map<NewArticleVm>(article);
            return articleVm;
        }

        public void UpdateArticle(NewArticleVm model)
        {
            var article = _mapper.Map<Article>(model);
            _articleRepo.UpdateArticle(article);

        }
    }
}
