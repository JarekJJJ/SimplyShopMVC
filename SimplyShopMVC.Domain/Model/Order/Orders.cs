using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Domain.Model.Order
{
    public class Orders
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string NumberOrders { get; set; }
        public string PaymentMethod { get; set; }
        public string DocumentType { get; set; }
        public string ShipingDescription { get; set; } = "brak";
        public bool IsAccepted { get; set; } = false;
        public bool IsCompleted { get; set; } = false;
        public bool IsCancelled { get; set; } = false;
        public ICollection<OrderItems> OrderItemss { get; set; }
    }
}
