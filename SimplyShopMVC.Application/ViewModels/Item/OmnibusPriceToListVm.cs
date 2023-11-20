using AutoMapper;
using SimplyShopMVC.Application.Mapping;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Application.ViewModels.Item
{
    public class OmnibusPriceToListVm : IMapFrom<SimplyShopMVC.Domain.Model.OmnibusPrice>
    {
        public int Id { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal PriceN { get; set; }
        public DateTime ChangeTime { get; set; }
        public string Ean { get; set; }
        public int WarehouseId { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<OmnibusPriceToListVm, SimplyShopMVC.Domain.Model.OmnibusPrice>().ReverseMap();
        }
    }
}
