using AutoMapper;
using SimplyShopMVC.Application.Mapping;
using SimplyShopMVC.Domain.Model.Sets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Application.ViewModels.PcSets
{
    public class PcSetsForListVm : IMapFrom<SimplyShopMVC.Domain.Model.Sets.PcSets>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int GroupItemId { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; } = DateTime.Now;
        public bool IsActive { get; set; }
        public bool IsSaved { get; set; }
        public bool IsDeleted { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<PcSetsForListVm, SimplyShopMVC.Domain.Model.Sets.PcSets>().ReverseMap();
        }
    }
}
