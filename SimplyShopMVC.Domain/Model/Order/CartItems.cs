using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Domain.Model.Order
{
    public class CartItems
    {
        public int Id { get; set; }
        public int CartId { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal PriceN { get; set; }
        public int VatRateId { get; set; }
        public int WarehouseId { get; set; }

        public virtual Cart Cart { get; set; }
        public virtual Item Item { get; set; }
        public virtual VatRate VatRate { get; set; }
        public virtual Warehouse Warehouse { get;set; }
    }
}
