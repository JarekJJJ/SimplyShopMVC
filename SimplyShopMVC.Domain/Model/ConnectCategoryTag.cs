using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Domain.Model
{
    public class ConnectCategoryTag
    {
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public int ItemTagId { get; set; }
        public ItemTag ItemTag { get; set; }
    }
}
