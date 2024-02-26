using AutoMapper;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SimplyShopMVC.Application.Mapping;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Application.ViewModels.Suppliers 
{
    public class OrinkItemForListVm : IMapFrom<SimplyShopMVC.Domain.Model.Suppliers.Orink>
    {
        public int Id { get; set; }
        public int warehouseId { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string color { get; set; }
        public int OrinkGroupId { get; set; }
        public string printerProducent { get; set; }
        public string symbol_producenta { get; set; }
        public string ean { get; set; }
        public int stan_magazynowy { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal cenaNetto { get; set; }
        public string imgLink { get; set; } = "";
        public DateTime createDate { get; set; }
        public DateTime updateTime { get; set; } = DateTime.Now;
        public void Mapping(Profile profile)
        {
            profile.CreateMap<OrinkItemForListVm, SimplyShopMVC.Domain.Model.Suppliers.Orink>().ReverseMap();
        }
    }
}
