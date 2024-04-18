using SimplyShopMVC.Domain.Model.Suppliers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Domain.Model
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public bool IsMainCategory { get; set; }
        public bool IsActive { get; set; }
        public int? MainCategoryId { get; set; }  
        public int? GroupItemId { get; set; }
        public GroupItem GroupItem { get; set; }
        public  ICollection<IncomGroup> IncomGroups { get; set; }
        public virtual ICollection<Item> Items { get; set; }
        public ICollection<ConnectCategoryGroup> ConnectCategoryGroups { get; set; }
        public ICollection<ConnectCategoryTag> ConnectCategoryTag { get; set; }
    }
}
