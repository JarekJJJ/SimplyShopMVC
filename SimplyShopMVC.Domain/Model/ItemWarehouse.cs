using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        public int Quantity { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? NetPurchasePrice { get; set; }
        //public int VatRateId { get; set; } //zapisuje się id stawki z modelu VatRate
        public string VatRateName { get; set; }// A-23/%, B-8% itd...
        [Column(TypeName = "decimal(18, 2)")]
        public decimal FinalPriceA { get; set; }

        public virtual Item Item { get; set; }
        public virtual Warehouse Warehouse { get; set; }
        public virtual VatRate VatRate { get; set; }

    }
}
