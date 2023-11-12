using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Domain.Model.Suppliers
{
    public class IncomGroup
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public int GroupIdHome { get; set; }
        public string Name { get; set; }
        public ICollection<Incom> Incoms { get; set; }
    }
}
