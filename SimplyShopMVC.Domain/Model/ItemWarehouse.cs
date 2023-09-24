using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Domain.Model
{
    // Tabela przechowuje informacje o stanach przedmiotu, cenie oraz czasie wysyłki w danym magazynie
    public class ItemWarehouse
    {
        public int Id { get; set; }
        public int WarehouseId { get; set; }
        public int ItemId { get; set; }
        public string WName { get; set; }
        public string? WDescription { get; set; }
        public int Quantity { get; set; }
        public int DeliveryTime { get; set; }
        public decimal? NetPurchasePrice { get; set; }
        public decimal FinalPriceA { get; set; }

        public virtual Item Item { get; set; }
        public virtual Warehouse Warehouse { get; set; }

    }
}
