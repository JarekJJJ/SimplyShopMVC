using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using SimplyShopMVC.Application.Mapping;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Application.ViewModels.Article
{
    public class UpdateArticleVm : IMapFrom<SimplyShopMVC.Domain.Model.Article>
    {
        public int Id { get; set; }
        [DisplayName("Tytuł")]
        public string Title { get; set; }
        [StringLength(255, MinimumLength = 5), Required]
        public string ShortDescription { get; set; }
        public string Content { get; set; }
        public DateTime Created { get; set; }
        // pomocnicz
        public List<ArticleTagsForListVm> Tags { get; set; } // Do wyświetlania tagów przy tworzeniu artykułów.
        public List<ArticleTagsForListVm> SelectedTags { get; set; } // do dodawania tagów do artykułu w serwisie
        public List<int> NewSelectedTags { get; set; } 
        public List<IFormFile> Image { get; set; }
        public List<string> ImageUrl { get; set; }
        public List<PhotoArticleVm> ListImages { get; set; }
        public List<string> SelectedImage { get; set; }
        public string MainImage { get; set; } //ToDo dodać do modelu i zrobić migracje !
        public void Mapping(Profile profile)
        {
            //przy tworzeniu nowego obiektu mapujemy z Vm-a do modelu w domain !!! 
            profile.CreateMap<UpdateArticleVm, SimplyShopMVC.Domain.Model.Article>().ReverseMap()
                .ForMember(s => s.Image, opt => opt.Ignore())
                .ForMember(s => s.ImageUrl, opt => opt.Ignore())
                .ForMember(s => s.ListImages, opt => opt.Ignore())
                .ForMember(s => s.SelectedImage, opt => opt.Ignore())
                .ForMember(s => s.Tags, opt => opt.Ignore())
                .ForMember(s => s.SelectedTags, opt => opt.Ignore());
        }
    }
    public class UpdateArticleValidation : AbstractValidator<UpdateArticleVm>
    {
        public UpdateArticleValidation()
        {
            RuleFor(x => x.Id).NotNull();
            RuleFor(x => x.Title).MinimumLength(5).NotEmpty();
        }
    }
}

