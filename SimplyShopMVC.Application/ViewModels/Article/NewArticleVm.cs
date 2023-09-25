using AutoMapper;
using FluentValidation;
using SimplyShopMVC.Application.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Application.ViewModels.Article
{
    public class NewArticleVm : IMapFrom<SimplyShopMVC.Domain.Model.Article>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string Content { get; set; }
        public DateTime Created { get; set; }
        public void Mapping(Profile profile)
        {
            //przy tworzeniu nowego obiektu mapujemy z Vm-a do modelu w domain !!! 
            profile.CreateMap<NewArticleVm, SimplyShopMVC.Domain.Model.Article>();
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
