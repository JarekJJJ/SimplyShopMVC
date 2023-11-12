using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Domain.Model
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? ShortDescription { get; set; }
        public string? Description { get; set; }
        public bool IsDeleted { get; set; } = false;
        public bool IsActive { get; set; } = true;
        public int? CategoryId { get; set; }
        public int? GroupItemId { get; set; }
        public string? EanCode { get; set; }
        public string ItemSymbol { get; set; }
        public string? ImageFolder { get; set; }

        public virtual Category Category { get; set; }
        public ICollection<ConnectItemTag> ConnectItemTags { get; set; }
        public virtual ICollection<ItemWarehouse> ItemWarehouses { get; set; }
        public virtual GroupItem GroupItem { get; set; }

    }
}
