using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Application.ViewModels.Order
{
    public class ListFavoriteItemsForListVm
    {
       public List<FavoriteItemsForListVm> listFavoriteItemVm { get; set; }
        public int selectedFavoriteItemId { get; set; }
    }
}
