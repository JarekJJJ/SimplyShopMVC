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
        private readonly IWebHostEnvironment _hosting;
        public ArticleService(IArticleRepository articleRepo, IMapper mapper, IWebHostEnvironment hostingEnvironment)
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
            if (tags != null)
            {
                foreach (var stags in tags)
                {
                    var element = _articleRepo.GetArticleTagByTagId(stags);
                    _articleRepo.AddConnectionArticleTags(articleId, element);
                }
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
                Tags = GetAllTagsToList(0),
                Count = articles.Count
            };

            return articlesList;
        }

        public ListArticleForListVm GetAllArticleForList()
        {
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
                catch (Exception ex)
                {

                }

            }
            var articlesList = new ListArticleForListVm()
            {
                Articles = articles,
                Tags = GetAllTagsToList(0),
                Count = articles.Count
            };
            return articlesList;
        }
        public ListArticleForListVm GetAllArticleForList(string tagName)
        {
            var tagForList = _articleRepo.GetAllArticleTags().FirstOrDefault(t => t.Name == tagName);
            ListArticleForListVm articlesList = new ListArticleForListVm();
            if (tagForList != null)
            {
                articlesList = GetAllArticlesByTagId(tagForList.Id);
            }
            return articlesList;
        }

        public ArticleDetailVm GetArticleDetails(int articleId)
        {
            var article = _articleRepo.GetArticleById(articleId);
            var articleVm = _mapper.Map<ArticleDetailVm>(article);
            articleVm.Tags = new List<ArticleTagsForListVm>();
            articleVm.listArticle = new List<ArticleForListVm>();
            List<ArticleForListVm> newListArticle = new List<ArticleForListVm>();
            var _pathImage = $"{_hosting.WebRootPath}\\media\\articleimg\\{article.Id}\\";
            var imageToList = ImageHelper.AllImageFromPath(_pathImage).ToList();
            var articleTags = _articleRepo.GetConnectArticleTags(articleId).ToList();
            foreach (var tag in articleTags)
            {
                var articleByTag = GetAllArticlesByTagId(tag.ArticleTagId);
                if (articleByTag.Articles != null && articleByTag.Articles.Count > 0)
                {
                    var tempArticle = newListArticle.Any(a => a.Id == tag.ArticleId);
                    if (!tempArticle)
                    {
                        newListArticle.AddRange(articleByTag.Articles);
                    }
                }
                articleVm.Tags.Add(_mapper.Map<ArticleTagsForListVm>(_articleRepo.GetArticleTagByTagId(tag.ArticleTagId)));

            }
            articleVm.listArticle.AddRange(newListArticle);
            articleVm.imagePath = imageToList;
            return articleVm;
        }
        public ArticleDetailVm GetArticleDetailsByTag(string tagName)
        {
            var tagForList = _articleRepo.GetAllArticleTags().FirstOrDefault(t => t.Name == tagName);
            ArticleDetailVm articleVm = new ArticleDetailVm();
            if (tagForList != null)
            {
                articleVm = _mapper.Map<ArticleDetailVm>(_articleRepo.GetArticlesByTagId(tagForList.Id).FirstOrDefault());
                if (articleVm != null)
                {
                    var _pathImage = $"{_hosting.WebRootPath}\\media\\articleimg\\{articleVm.Id}\\";
                    var imageToList = ImageHelper.AllImageFromPath(_pathImage).ToList();
                    articleVm.imagePath = imageToList;

                }
            }
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
            articleVm.Tags = GetAllTagsToList(0);
            articleVm.SelectedTags = GetAllSelectedTagsForList(articleId);
            articleVm.ListImages = listImage;
            return articleVm;
        }

        public void UpdateArticle(UpdateArticleVm model, [FromServices] IWebHostEnvironment oHostingEnvironment, List<string> selectedImage)
        {
            var article = _mapper.Map<Article>(model);
            _articleRepo.UpdateArticle(article);
            var folderName = article.Id.ToString();
            string newFolderPath = Path.Combine(oHostingEnvironment.WebRootPath, "media\\articleimg", folderName);

            try
            {
                Directory.CreateDirectory(newFolderPath);
            }
            catch (Exception)
            {
                throw;
            }
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
        public List<ArticleTagsForListVm> GetAllTagsToList(int? numberTags)
        {
            List<ArticleTagsForListVm> listTags = new List<ArticleTagsForListVm>();
            if (numberTags == null || numberTags == 0)
            {
                var allTags = _articleRepo.GetAllArticleTags()
             .ProjectTo<ArticleTagsForListVm>(_mapper.ConfigurationProvider).ToList();
                foreach (var tag in allTags)
                {
                    listTags.Add(tag);
                }
                return listTags;
            }
            var tags = _articleRepo.GetAllArticleTags()
            .ProjectTo<ArticleTagsForListVm>(_mapper.ConfigurationProvider).ToList().Take((int)numberTags);
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

        public UpdateArticleTagVm ListArticleTagToUpdate(string? searchTag)
        {
            UpdateArticleTagVm updateArticleTagVm = new UpdateArticleTagVm();
            updateArticleTagVm.Count = new List<CountArticleVm>();

            if (searchTag == null)
            {
                updateArticleTagVm.TagList = GetAllTagsToList(20);
                foreach (var tags in updateArticleTagVm.TagList)
                {
                    CountArticleVm countArticle = new CountArticleVm();
                    var count = _articleRepo.GetArticlesByTagId(tags.Id).Count();
                    countArticle.count = count;
                    countArticle.articleTagId = tags.Id;

                    updateArticleTagVm.Count.Add(countArticle);
                }
                return updateArticleTagVm;
            }

            updateArticleTagVm.TagList = GetAllTagsToList(0).Where(t => t.Name.Contains(searchTag)).ToList();
            return updateArticleTagVm;

        }

        public void UpdateArticleTag(UpdateArticleTagVm articleTag, int options)
        {
            if (articleTag != null && options == 1)
            {
                var mapArticleTag = _mapper.Map<ArticleTag>(articleTag.Tag);
                _articleRepo.UpdateArticleTag(mapArticleTag);
            }
            if (articleTag != null && options == 2)
            {
                //var mapArticleTag = _mapper.Map<ArticleTag>(articleTag.Tag);
                _articleRepo.DeleteArticleTag(articleTag.Tag.Id);
            }
            if (articleTag != null && options == 3)
            {
                var mapArticleTag = _mapper.Map<ArticleTag>(articleTag.Tag);
                _articleRepo.AddArticleTag(mapArticleTag);
            }
        }

        public UpdateArticleTagVm GetArticleTagToUpdate(int tagId)
        {
            UpdateArticleTagVm updateArticleTagVm = new UpdateArticleTagVm();
            var result = _articleRepo.GetArticleTagByTagId(tagId);
            var tagMap = _mapper.Map<ArticleTagsForListVm>(result);
            updateArticleTagVm.Tag = tagMap;
            return updateArticleTagVm;

        }
    }
}
