using AutoMapper;
using AutoMapper.QueryableExtensions;
using SimplyShopMVC.Application.Interfaces;
using SimplyShopMVC.Application.ViewModels.Article;
using SimplyShopMVC.Domain.Interface;
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
            throw new NotImplementedException();
        }

        public int DeleteArticle(int id)
        {
            throw new NotImplementedException();
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

        public int UpdateArticle(NewArticleVm article)
        {
            throw new NotImplementedException();
        }
    }
}
