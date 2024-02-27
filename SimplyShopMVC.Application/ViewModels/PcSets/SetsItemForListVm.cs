using AutoMapper;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SimplyShopMVC.Application.Mapping;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Application.ViewModels.PcSets
{
    public class SetsItemForListVm : IMapFrom<SimplyShopMVC.Domain.Model.Sets.PcSetsItems>
    {
        public int Id { get; set; }
        public int PcSetsId { get; set; }
        public int ItemId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal NetPurchasePrice { get; set; }
        public int VatRateId { get; set; }
        public int WarehouseId { get; set; }
        public string EanCode { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<SetsItemForListVm, SimplyShopMVC.Domain.Model.Sets.PcSetsItems>().ReverseMap();
        }

    }
}
