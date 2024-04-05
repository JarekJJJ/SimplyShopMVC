using AutoMapper;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SimplyShopMVC.Application.Mapping;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Application.ViewModels.Order
{
    public class DeliveryForListVm: IMapFrom<SimplyShopMVC.Domain.Model.Order.Delivery>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Cost { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal MaxWeight { get; set; }
        public DateTime UpdateTime { get; set; } = DateTime.Now;
        public void Mapping(Profile profile)
        {
            profile.CreateMap<DeliveryForListVm, SimplyShopMVC.Domain.Model.Order.Delivery>().ReverseMap();
        }
    }
}
