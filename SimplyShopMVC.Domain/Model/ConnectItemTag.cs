using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Domain.Model
{
    public class ConnectItemTag
    {
        public int ItemId { get; set; }
        public Item Item { get; set; }
        public int ItemTagId { get; set; }
        public ItemTag ItemTag { get; set; }
    }
}
