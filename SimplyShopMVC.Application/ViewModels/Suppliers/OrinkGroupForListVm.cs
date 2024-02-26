using AutoMapper;
using SimplyShopMVC.Application.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Application.ViewModels.Suppliers
{
    public class OrinkGroupForListVm: IMapFrom<SimplyShopMVC.Domain.Model.Suppliers.OrinkGroup>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int HomeGroupId { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<OrinkGroupForListVm, SimplyShopMVC.Domain.Model.Suppliers.OrinkGroup>().ReverseMap();
        }
    }
}
