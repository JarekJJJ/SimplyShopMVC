using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SimplyShopMVC.Application.Interfaces;
using SimplyShopMVC.Application.ViewModels.Article;
using SimplyShopMVC.Domain.Interface;
using SimplyShopMVC.Domain.Model;
using SimplyShopMVC.Application.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Application.Services
{
    public class ArticleService : IArticleService
    {
        private readonly IArticleRepository _articleRepo;
        private readonly IMapper _mapper;
        private readonly IHostingEnvironment _hosting;
        public ArticleService(IArticleRepository articleRepo, IMapper mapper, IHostingEnvironment hostingEnvironment)
        {
            _articleRepo = articleRepo;
            _mapper = mapper;
            _hosting = hostingEnvironment;
        }

        public int AddArticle(NewArticleVm article, [FromServices] IHostingEnvironment oHostingEnvironment)
        {
            var art = _mapper.Map<Article>(article); //Wskazuje docelowy model w domain i przekazuje obiekt article
            var id = _articleRepo.AddArticle(art);
            var folderName = art.Id.ToString();
            string newFolderPath = Path.Combine(oHostingEnvironment.WebRootPath, "media\\articleimg", folderName);
            Directory.CreateDirectory(newFolderPath);
            foreach (var file in article.Image)
            {
                string fileName = $"{file.FileName}";
                string filePath = System.IO.Path.Combine(newFolderPath, fileName);
                using (FileStream fileStream = System.IO.File.Create(filePath))
                {
                    file.CopyTo(fileStream);
                    fileStream.Flush();
                }
            }
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
            foreach (var article in articles)
            {
                var _pathImage = $"{_hosting.WebRootPath}\\media\\articleimg\\{article.Id}\\";
                var imageToList = ImageHelper.AllImageFromPath(_pathImage).Take(1).ToList();
                //Pobierane są wszystkie zdjęcia z folderu o id artykułu i przypisywane do listy Viewmodelu
                article.imagePath = imageToList;
            }
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
            var _pathImage = $"{_hosting.WebRootPath}\\media\\articleimg\\{article.Id}\\";
            var imageToList = ImageHelper.AllImageFromPath(_pathImage).ToList();
            articleVm.imagePath = imageToList;
            return articleVm;
        }

        public UpdateArticleVm GetArticleToUpdate(int articleId)
        {
            var article = _articleRepo.GetArticleById(articleId);
            var articleVm = _mapper.Map<UpdateArticleVm>(article);
            var _pathImage = $"{_hosting.WebRootPath}\\media\\articleimg\\{article.Id}\\";
            var imageToList = ImageHelper.AllImageFromPath(_pathImage).ToList();
            articleVm.ImageUrl = new List<string>();
            foreach (var imageUrl in imageToList)
            {
                var _imgUrl = $"/media/articleimg/{articleId}/{imageUrl}";
                articleVm.ImageUrl.Add(_imgUrl);
           
            }          
            return articleVm;
        }

        public void UpdateArticle(UpdateArticleVm model, [FromServices] IHostingEnvironment oHostingEnvironment)
        {
            var article = _mapper.Map<Article>(model);
            _articleRepo.UpdateArticle(article);
            var folderName = article.Id.ToString();
            string newFolderPath = Path.Combine(oHostingEnvironment.WebRootPath, "media\\articleimg", folderName);
           // Directory.CreateDirectory(newFolderPath);
            if (model.Image != null)
            {
                foreach (var image in model.Image)
                {
                    string fileName = $"{image.FileName}";
                    string filePath = System.IO.Path.Combine(newFolderPath, fileName);
                    using (FileStream fileStream = System.IO.File.Create(filePath))
                    {
                        image.CopyTo(fileStream);
                        fileStream.Flush();
                    }
                }
            }

        }

    }
}
