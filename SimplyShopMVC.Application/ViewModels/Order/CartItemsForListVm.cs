using SimplyShopMVC.Application.Mapping;
using SimplyShopMVC.Domain.Model.Order;
using SimplyShopMVC.Domain.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace SimplyShopMVC.Application.ViewModels.Order
{
    public class CartItemsForListVm : IMapFrom<SimplyShopMVC.Domain.Model.Order.CartItems>
    {
        public int Id { get; set; }
        public int CartId { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal PriceN { get; set; }
        public int VatRateId { get; set; }
        public int WarehouseId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CartItemsForListVm, SimplyShopMVC.Domain.Model.Order.CartItems>().ReverseMap();
        }
    }
}
