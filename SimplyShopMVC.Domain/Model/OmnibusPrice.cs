using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Domain.Model
{
    public class OmnibusPrice
    {
        public int Id { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal PriceN { get; set; }
        public DateTime ChangeTime { get; set; }
        public string Ean { get; set; }
        public int WarehouseId { get; set; }
        public virtual Warehouse Warehouse { get; set;}


    }
}
