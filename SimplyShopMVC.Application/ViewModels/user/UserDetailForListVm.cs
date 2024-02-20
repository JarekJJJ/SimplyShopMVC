using AutoMapper;
using SimplyShopMVC.Application.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Application.ViewModels.user
{
    public class UserDetailForListVm: IMapFrom<SimplyShopMVC.Domain.Model.users.UserDetail>
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string? FullName { get; set; }
        public bool? IsClientBusiness { get; set; }
        public string? ContactPerson { get; set; }
        public string? NIP { get; set; }
        public string? EmailAddress { get; set; }
        public string? PhoneNumber { get; set; }
        public string? City { get; set; }
        public string? Region { get; set; }
        public string? PostalCode { get; set; }
        public string? Country { get; set; }
        public string? Street { get; set; }
        public string? PriceLevel { get; set; }
        public bool IsActive { get; set; } = false;
        public bool IsBlocked { get; set; } = false;
        public void Mapping(Profile profile)
        {
            profile.CreateMap<UserDetailForListVm, SimplyShopMVC.Domain.Model.users.UserDetail>().ReverseMap();
        }
    }
}
