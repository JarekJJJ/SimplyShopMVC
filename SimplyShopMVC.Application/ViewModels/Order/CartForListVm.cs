using AutoMapper;
using SimplyShopMVC.Application.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Application.ViewModels.Order
{
    public class CartForListVm : IMapFrom<SimplyShopMVC.Domain.Model.Order.Cart>
    {
        public int Id { get; set; }
        public string userId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? RealizedDate { get; set; }
        public bool IsDeleted { get; set; } = false;
        public bool IsRealized { get; set; } = false;
        public bool IsSaved { get; set; } = false;
        public void Mapping(Profile profile)
        {
            profile.CreateMap<CartForListVm, SimplyShopMVC.Domain.Model.Order.Cart>().ReverseMap();
        }
    }
}
