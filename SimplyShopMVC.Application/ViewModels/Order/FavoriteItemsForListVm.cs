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
    public class FavoriteItemsForListVm : IMapFrom<SimplyShopMVC.Domain.Model.Order.FavoriteItem>
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public string UserId { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal PriceB { get; set; }
        public DateTime AddDate { get; set; }
        public decimal actualPriceB { get; set; }
        public int actualQuantity { get; set; }
        public int? warehouseId { get; set; }
        public int? VatRateId { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<FavoriteItemsForListVm, SimplyShopMVC.Domain.Model.Order.FavoriteItem>().ReverseMap()
                  .ForMember(s => s.actualPriceB, opt => opt.Ignore())
                  .ForMember(s => s.actualQuantity, opt => opt.Ignore())
                  .ForMember(s => s.warehouseId, opt => opt.Ignore())
                  .ForMember(s => s.VatRateId, opt => opt.Ignore());

        }
    }
}
