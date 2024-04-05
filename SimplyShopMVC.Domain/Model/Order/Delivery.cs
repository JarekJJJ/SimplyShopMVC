using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Domain.Model.Order
{
    public class Delivery
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Cost { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal MaxWeight { get; set; }
        public DateTime UpdateTime { get; set; } = DateTime.Now;
        public virtual ICollection<Orders> Orders { get; set; }
    }
}
