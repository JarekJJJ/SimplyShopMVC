using SimplyShopMVC.Application.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace SimplyShopMVC.Application.ViewModels.Article
{
    public class ArticleDetailVm : IMapFrom<SimplyShopMVC.Domain.Model.Article>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string Content { get; set; }
        public DateTime Created { get; set; }
        public int Priority { get; set; }
        public List<string> imagePath { get; set; }
        public List<ArticleForListVm> listArticle { get; set; }
        public List<ArticleTagsForListVm> Tags { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<SimplyShopMVC.Domain.Model.Article, ArticleDetailVm>()
                .ForMember(s => s.imagePath, opt => opt.Ignore())
            .ForMember(s => s.listArticle, opt => opt.Ignore())
            .ForMember(s => s.Tags, opt => opt.Ignore());
        }
    }

}
