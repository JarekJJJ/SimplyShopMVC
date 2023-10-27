using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Domain.Model
{
    public class VatRate
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Value { get; set; }

        public virtual ICollection<ItemWarehouse> ItemWarehouses { get; set; }

    }
}
