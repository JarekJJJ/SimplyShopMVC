using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Domain.Model.Suppliers
{
    public class ConnectCategoryGroup
    {
        public int Id { get; set; }
        public Category Category { get; set; } 
        public int CategoryId { get; set; }
        public List<IncomGroup> IncomGroups { get; set; } 
        public int GroupItemId { get; set; }
        public GroupItem GroupItem { get; set; } 
        public int? ItemTagId { get; set; }
        public ItemTag ItemTag { get; set; } 
    }
}
