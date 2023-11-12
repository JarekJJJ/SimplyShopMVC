using AutoMapper;
using SimplyShopMVC.Application.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Application.ViewModels.Item
{
    public class GroupItemForListVm : IMapFrom<SimplyShopMVC.Domain.Model.GroupItem>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int PriceMarkupA { get; set; }
        public int PriceMarkupB { get; set; }
        public int PriceMarkupC { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<GroupItemForListVm, SimplyShopMVC.Domain.Model.GroupItem>().ReverseMap();
        }
    }
}
