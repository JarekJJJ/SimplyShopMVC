using SimplyShopMVC.Domain.Model.Sets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Domain.Model
{
    public class GroupItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int PriceMarkupA { get; set; }
        public int PriceMarkupB { get;set; }
        public int PriceMarkupC { get;set; }
        public ICollection<Category> Categories { get; set; }
        public ICollection<Item> Items { get; set; }
        public ICollection<PcSets> PcSets { get; set; }
    }
}
