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
        public Category Category { get; set; } = new Category();
        public int CategoryId { get; set; }
        public int IncomGroupId { get; set; }
        public IncomGroup IncomGroup { get; set; } = new IncomGroup();
        public int GroupItemId { get; set; }
        public GroupItem GroupItem { get; set; } = new GroupItem();
        public int? ItemTagId { get; set; }
        public ItemTag ItemTag { get; set; } = new ItemTag();
    }
}
