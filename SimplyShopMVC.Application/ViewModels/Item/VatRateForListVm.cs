using AutoMapper;
using SimplyShopMVC.Application.Mapping;
using SimplyShopMVC.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Application.ViewModels.Item
{
    public class VatRateForListVm : IMapFrom<SimplyShopMVC.Domain.Model.VatRate>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Value { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<VatRateForListVm, SimplyShopMVC.Domain.Model.VatRate>().ReverseMap();
        }
    }
}
