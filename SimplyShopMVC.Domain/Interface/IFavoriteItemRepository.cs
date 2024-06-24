using SimplyShopMVC.Domain.Model.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Domain.Interface
{
    public interface IFavoriteItemRepository
    {
        int AddFavoriteItem(FavoriteItem favoriteItem);
        void UpdateFavoriteItem(FavoriteItem favoriteItems);
        void DeleteFavoriteItem(int favoriteItemId);
        IQueryable<FavoriteItem> GetAllFavoriteItems();
    }
}
