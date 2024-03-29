﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using SimplyShopMVC.Application.ViewModels.Article;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Application.Interfaces
{
    public interface IArticleService
    {
        ListArticleForListVm GetAllArticleForList();
        ListArticleForListVm GetAllArticleForList(string tagName);
        ListArticleForListVm GetAllArticlesByTagId(int tagId);
        ArticleDetailVm GetArticleDetails(int articleId);
        ArticleDetailVm GetArticleDetailsByTag(string tagName);
        UpdateArticleVm GetArticleToUpdate(int articleId);
        NewArticleVm AddArticle(NewArticleVm article, [FromServices] IHostingEnvironment oHostingEnvironment);
        void UpdateArticle(UpdateArticleVm article, [FromServices] IWebHostEnvironment oHostingEnvironment, List<string> selectedImage);
        void DeleteArticle(int id);
        int AddTag(NewArticleVm model);
        UpdateArticleTagVm ListArticleTagToUpdate(string? searchTag);
        UpdateArticleTagVm GetArticleTagToUpdate(int tagId);
        void UpdateArticleTag(UpdateArticleTagVm articleTag, int options);
    }
}
