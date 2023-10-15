using AutoMapper;
using SimplyShopMVC.Application.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Application.ViewModels.Item
{
    public class ItemForListVm :IMapFrom<SimplyShopMVC.Domain.Model.Item>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? ShortDescription { get; set; }
        public string? Description { get; set; }
        public bool IsDeleted { get; set; } = false;
        public bool IsActive { get; set; } = true;
        public int CategoryId { get; set; }
        public string? EanCode { get; set; }
        public string ItemSymbol { get; set; }
        public int VatRate { get; set; }
        public string ImageFolder { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ItemForListVm, SimplyShopMVC.Domain.Model.Item>().ReverseMap();
        }
    }
}
