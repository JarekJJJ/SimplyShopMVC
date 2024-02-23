using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Domain.Model.Suppliers
{
    public class OrinkGroup
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int HomeGroupId { get; set; }
        public virtual ICollection<Orink> Orinks { get; set; }
    }
}
