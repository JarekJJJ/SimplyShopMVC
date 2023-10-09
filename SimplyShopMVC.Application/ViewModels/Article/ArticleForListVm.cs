using AutoMapper;
using Microsoft.AspNetCore.Identity;
using SimplyShopMVC.Application.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Application.ViewModels.Article
{
    public class ArticleForListVm: IMapFrom<SimplyShopMVC.Domain.Model.Article>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public DateTime Created { get; set; }
        // Właściwości pomocnicze poza modelem
        public List<ArticleTagsForListVm>? artTags { get; set; }
        public List<string>? imagePath { get; set; }        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<SimplyShopMVC.Domain.Model.Article, ArticleForListVm>()
                .ForMember(s=>s.imagePath,opt=>opt.Ignore())
                .ForMember(s => s.artTags, opt => opt.Ignore());
        }
    }
}
