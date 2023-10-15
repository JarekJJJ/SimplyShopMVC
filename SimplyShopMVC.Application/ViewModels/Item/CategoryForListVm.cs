using AutoMapper;
using SimplyShopMVC.Application.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Application.ViewModels.Item
{
    public class CategoryForListVm : IMapFrom<SimplyShopMVC.Domain.Model.Category>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public bool IsMainCategory { get; set; }
        public int? MainCategoryId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CategoryForListVm, SimplyShopMVC.Domain.Model.Category>().ReverseMap();

        }
    }
}
