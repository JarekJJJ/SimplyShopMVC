using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using SimplyShopMVC.Application.Mapping;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Application.ViewModels.Article
{
    public class NewArticleVm : IMapFrom<SimplyShopMVC.Domain.Model.Article>
    {
        public int Id { get; set; }
        [DisplayName("Tytuł")]
        public string Title { get; set; }
        [StringLength(255, MinimumLength =5), Required]
        public string ShortDescription { get; set; }
        public string Content { get; set; }
        public DateTime Created { get; set; }
        public IFormFile Image { get; set; }  
        public void Mapping(Profile profile)
        {
            //przy tworzeniu nowego obiektu mapujemy z Vm-a do modelu w domain !!! 
            profile.CreateMap<NewArticleVm, SimplyShopMVC.Domain.Model.Article>().ReverseMap()
                .ForMember(s=>s.Image, opt=>opt.Ignore());
        }
    }
    public class NewArticleValidation : AbstractValidator<NewArticleVm>
    {
        public NewArticleValidation()
        {
            RuleFor(x => x.Id).NotNull();
            RuleFor(x => x.Title).MinimumLength(5).NotEmpty();
        }
    }
}
