using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Domain.Model
{
    public class ItemTag
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public ICollection<ConnectItemTag> ConnectItemTags { get; set; }
        public ICollection<ConnectCategoryTag> ConnectCategoryTags{ get; set; }


    }
}
