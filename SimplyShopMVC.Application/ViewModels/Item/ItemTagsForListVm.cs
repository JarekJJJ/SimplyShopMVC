using AutoMapper;
using SimplyShopMVC.Application.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Application.ViewModels.Item
{
    public class ItemTagsForListVm : IMapFrom<SimplyShopMVC.Domain.Model.ItemTag>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ItemTagsForListVm, SimplyShopMVC.Domain.Model.ItemTag>().ReverseMap();

        }

    }
}
