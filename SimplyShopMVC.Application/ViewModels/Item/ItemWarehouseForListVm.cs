using AutoMapper;
using SimplyShopMVC.Application.Mapping;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Application.ViewModels.Item
{
    public class ItemWarehouseForListVm : IMapFrom<SimplyShopMVC.Domain.Model.ItemWarehouse>
    {
        public int Id { get; set; }
        public int WarehouseId { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        [DataType(DataType.Currency)]
        public decimal? NetPurchasePrice { get; set; }
        public int VatRate { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        [DataType(DataType.Currency)]
        public decimal FinalPriceA { get; set; } //Dodać B i C
        public void Mapping(Profile profile)
        {
            profile.CreateMap<ItemWarehouseForListVm, SimplyShopMVC.Domain.Model.ItemWarehouse>().ReverseMap();

        }
    }

}
