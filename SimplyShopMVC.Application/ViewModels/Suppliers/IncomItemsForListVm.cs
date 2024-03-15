using AutoMapper;
using SimplyShopMVC.Application.Mapping;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Application.ViewModels.Suppliers
{
    public class IncomItemsForListVm: IMapFrom<SimplyShopMVC.Domain.Model.Suppliers.Incom>
    {
        public int Id { get; set; }
        public int warehouseId { get; set; }
        public string grupa_towarowa { get; set; }
        public string nazwa_grupy_towarowej { get; set; }
        public string symbol_produktu { get; set; }
        public string nazwa_produktu { get; set; }
        public string symbol_producenta { get; set; }
        public string ean { get; set; }
        public string nazwa_producenta { get; set; }
        public string opis { get; set; }
        public string urlImage { get; set; }
        public int stan_magazynowy { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal cena { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal dlugosc { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal szerokosc { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal wysokosc { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal waga { get; set; }
        public DateTime createDate { get; set; }
        public DateTime updateTime { get; set; }
       
        public void Mapping(Profile profile)
        {
            profile.CreateMap<IncomItemsForListVm, SimplyShopMVC.Domain.Model.Suppliers.Incom>().ReverseMap();
        }
    }
}
