using AutoMapper;
using SimplyShopMVC.Application.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Application.ViewModels.Item
{
    public class WarehouseForListVm : IMapFrom<SimplyShopMVC.Domain.Model.Warehouse>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int DeliveryTime { get; set; }
        public bool IsActive { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<WarehouseForListVm, SimplyShopMVC.Domain.Model.Warehouse>().ReverseMap();
        }
    }
}
