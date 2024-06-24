using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Domain.Model.Order
{
    public class FavoriteItem
    {
        public int Id { get; set; }
        public virtual Item Item { get; set; }
        public int ItemId { get; set; }
        public string UserId { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal PriceB { get; set; }
        public DateTime AddDate { get; set; }

    }
}
