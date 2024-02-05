using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Domain.Model.Order
{
    public class OrderItems
    {
        public int Id { get; set; }
        public int OrdersId { get; set; }
        public int ItemId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal PriceB { get; set; }
        public int VatRateId { get; set; }
        public int WarehouseId { get; set; }
        public string EanCode { get; set; }

        public virtual Orders? Orders { get; set; }
        public virtual Item Item { get; set; }
        public virtual VatRate VatRate { get; set; }
        public virtual Warehouse Warehouse { get; set; }
    }
}
