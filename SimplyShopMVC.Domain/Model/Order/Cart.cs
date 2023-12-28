using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Domain.Model.Order
{
    public class Cart
    {
        public int Id { get; set; }
        public string userId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime RealizedDate { get; set;}
        public bool IsDeleted { get; set; } = false;
        public bool IsRealized { get; set; } = false;
        public virtual ICollection<CartItems> CartItems { get; set; }
    }
}
