using AutoMapper;
using SimplyShopMVC.Application.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Application.ViewModels.Item
{
    public class AddItemVm : IMapFrom<SimplyShopMVC.Domain.Model.Item>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? ShortDescription { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public int CategoryId { get; set; }
        public string? EanCode { get; set; }
        public string ItemSymbol { get; set; }
        public int VatRate { get; set; }
        public string ImageFolder { get; set; }
        // pomocnicze
        public List<ItemTagsForListVm> ItemTags { get; set; }
        public List<int> SelectedTags { get; set; } // do dodawania tagów do towaru w serwisie
        public int TagId { get; set; }
        public string TagName { get; set; }
        public string TagDescription { get; set; }


        public void Mapping(Profile profile)
        {
            //przy tworzeniu nowego obiektu mapujemy z Vm-a do modelu w domain !!! 
            profile.CreateMap<AddItemVm, SimplyShopMVC.Domain.Model.Item>().ReverseMap()
                            .ForMember(s => s.ItemTags, opt => opt.Ignore())
                            .ForMember(s => s.TagId, opt => opt.Ignore())
                            .ForMember(s => s.TagName, opt => opt.Ignore())
                            .ForMember(s => s.TagDescription, opt => opt.Ignore())
                            .ForMember(s => s.SelectedTags, opt => opt.Ignore());
        }
    }
}
