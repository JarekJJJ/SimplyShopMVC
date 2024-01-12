using AutoMapper;
using SimplyShopMVC.Application.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Application.ViewModels.user
{
    public class CompanySettingsVm : IMapFrom<SimplyShopMVC.Domain.Model.users.CompanySettings>
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string NIP { get; set; }
        public string? Regon { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string PostCode { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string BankName { get; set; }
        public string BankAccount { get; set; }
        public string? AdditionalInfo { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<CompanySettingsVm, SimplyShopMVC.Domain.Model.users.CompanySettings>().ReverseMap();
        }
    }
}
