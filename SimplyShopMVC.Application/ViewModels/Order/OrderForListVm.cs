using AutoMapper;
using SimplyShopMVC.Application.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Application.ViewModels.Order
{
    public class OrderForListVm: IMapFrom<SimplyShopMVC.Domain.Model.Order.Orders>
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string NumberOrders { get; set; }
        public string PaymentMethod { get; set; }
        public string DocumentType { get; set; }
        public string ShipingDescription { get; set; }
        public bool IsAccepted { get; set; } = false;
        public void Mapping(Profile profile)
        {
            profile.CreateMap<OrderForListVm, SimplyShopMVC.Domain.Model.Order.Orders>().ReverseMap();
        }
    }
}
