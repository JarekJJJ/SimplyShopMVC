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
    public class OrderItemsForListVm: IMapFrom<SimplyShopMVC.Domain.Model.Order.OrderItems>
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ItemId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal PriceB { get; set; }
        public int VatRateId { get; set; }
        public int WarehouseId { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<OrderItemsForListVm, SimplyShopMVC.Domain.Model.Order.OrderItems>().ReverseMap();
        }
    }
}
