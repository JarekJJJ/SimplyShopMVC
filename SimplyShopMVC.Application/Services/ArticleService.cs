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
using System.ComponentModel;

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
        public NewArticleVm AddArticle(NewArticleVm article, [FromServices] IHostingEnvironment? oHostingEnvironment)
        {
            var art = _mapper.Map<Article>(article); //Wskazuje docelowy model w domain i przekazuje obiekt article

            if (article.Title != null) // przy dodawaniu artykułu - validation nie pozwoli dodać artykułu bez tytułu dlatego można sprawdzić jaki formularz jest wysyłany czy add article czy add tag
            {
                List<ArticleTag> tags = new List<ArticleTag>();
                var id = _articleRepo.AddArticle(art);                
                var folderName = art.Id.ToString();
                string newFolderPath = Path.Combine(oHostingEnvironment.WebRootPath, "media\\articleimg", folderName);
                Directory.CreateDirectory(newFolderPath);
                if (article.Image != null)
                {
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
                }
                AddTagsToArticle(article.SelectedTags, id);
            }
            else
            {
                List<ArticleTagsForListVm> listTags = new List<ArticleTagsForListVm>();
                var tags = _articleRepo.GetAllArticleTags()
                    .ProjectTo<ArticleTagsForListVm>(_mapper.ConfigurationProvider).ToList();
                foreach (var tag in tags)
                {
                    listTags.Add(tag);
                }
                article.Tags = listTags;

            }
            return article;
        }

        public int AddTag(NewArticleVm model)
        {
            //
            var listAvailableTags = _articleRepo.GetAllArticleTags().Where(t => t.Name.Equals(model.TagName)).Count();
            if (model.TagName != null && listAvailableTags == 0) // przy dodawaniu samego tagu bez dodawania artykułu - formularz AddTags
            {
                var tags = new ArticleTagsForListVm()
                {
                    Id = model.TagId,
                    Name = model.TagName,
                    Description = model.TagDescription,
                };
                var tagMap = _mapper.Map<ArticleTag>(tags);
                //result.Tags.Add(tags);
                var id = _articleRepo.AddArticleTag(tagMap);
                return id;
            }
            return 0;
        }
        public void AddTagsToArticle(List<int> tags, int articleId)
        {
            _articleRepo.DeleteConnectionArticleTags(articleId);
            foreach (var stags in tags)
            {
                var element = _articleRepo.GetArticleTagByTagId(stags);
                _articleRepo.AddConnectionArticleTags(articleId, element);
            }
        }

        public void DeleteArticle(int id)
        {
            _articleRepo.DeleteArticle(id);
        }
        public ListArticleForListVm GetAllArticlesByTagId(int tagId)
        {
            var articles = _articleRepo.GetArticlesByTagId(tagId)
                 .ProjectTo<ArticleForListVm>(_mapper.ConfigurationProvider).ToList();
            foreach (var article in articles)
            {
                try
                {
                    var _pathImage = $"{_hosting.WebRootPath}\\media\\articleimg\\{article.Id}\\";
                    var imageToList = ImageHelper.AllImageFromPath(_pathImage).Take(1).ToList();
                    //Pobierane są wszystkie zdjęcia z folderu o id artykułu i przypisywane do listy Viewmodelu
                    article.imagePath = imageToList;

                    article.artTags = GetAllSelectedTagsForList(article.Id);
                }
                catch (Exception ex)
                {

                }

            }
            var articlesList = new ListArticleForListVm()
            {
                Articles = articles,
                Tags = GetAllTagsToList(),
                Count = articles.Count
            };

            return articlesList;
        }

        public ListArticleForListVm GetAllArticleForList()
        {
            //.projectTo - stosujemy przy kolekcjacj Iquerable
            var articles = _articleRepo.GetAllArticles()
                  .ProjectTo<ArticleForListVm>(_mapper.ConfigurationProvider).ToList();

            foreach (var article in articles)
            {
                try
                {
                    var _pathImage = $"{_hosting.WebRootPath}\\media\\articleimg\\{article.Id}\\";
                    var imageToList = ImageHelper.AllImageFromPath(_pathImage).Take(1).ToList();
                    //Pobierane są wszystkie zdjęcia z folderu o id artykułu i przypisywane do listy Viewmodelu
                    article.imagePath = imageToList;

                    article.artTags = GetAllSelectedTagsForList(article.Id);
                }
                catch(Exception ex)
                {

                }
               
            }
            var articlesList = new ListArticleForListVm()
            {
                Articles = articles,
                Tags = GetAllTagsToList(),
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
            var listImage = new List<PhotoArticleVm>();
            int photoId = 0;
            foreach (var imageUrl in imageToList)
            {
                var photoDetail = new PhotoArticleVm();
                photoDetail.Id = photoId;
                photoId++;
                photoDetail.Name = imageUrl;
                var _imgFullUrl = $"/media/articleimg/{articleId}/{imageUrl}";
                photoDetail.ImageUrl = _imgFullUrl;
                photoDetail.IsSelected = false;
                listImage.Add(photoDetail);
            }
            articleVm.Tags = GetAllTagsToList();
            articleVm.SelectedTags = GetAllSelectedTagsForList(articleId);
            articleVm.ListImages = listImage;
            return articleVm;
        }

        public void UpdateArticle(UpdateArticleVm model, [FromServices] IHostingEnvironment oHostingEnvironment, List<string> selectedImage)
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
            if (selectedImage != null)
            {
                foreach (var _image in selectedImage)
                {
                    var result = ImageHelper.DeleteImage(_image, newFolderPath);
                }
            }
            AddTagsToArticle(model.NewSelectedTags, model.Id);
        }
        public List<ArticleTagsForListVm> GetAllTagsToList()
        {
            List<ArticleTagsForListVm> listTags = new List<ArticleTagsForListVm>();
            var tags = _articleRepo.GetAllArticleTags()
                .ProjectTo<ArticleTagsForListVm>(_mapper.ConfigurationProvider).ToList();
            foreach (var tag in tags)
            {
                listTags.Add(tag);
            }
            return listTags;
        }
        public List<ArticleTagsForListVm> GetAllSelectedTagsForList(int articleId)
        {
            var innerList = new List<ArticleTagsForListVm>();
            var tempArticle = _articleRepo.GetArticleById(articleId);
            var tempListTag = tempArticle.ConnectArticleTags.Where(a => a.ArticleId == articleId);
            if (tempListTag.Count() >= 1)
            {
                foreach (var t in tempListTag)
                {
                    ArticleTagsForListVm newList = new ArticleTagsForListVm();
                    var tag = _articleRepo.GetArticleTagByTagId(t.ArticleTagId);
                    newList.Id = tag.Id;
                    newList.Name = tag.Name;
                    newList.Description = tag.Description;
                    innerList.Add(newList);
                }
            }
            return innerList;
        }
    }
}
