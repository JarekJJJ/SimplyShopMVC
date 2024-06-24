using SimplyShopMVC.Domain.Interface;
using SimplyShopMVC.Domain.Model.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Infrastructure.Repositories
{
    public class FavoriteItemRepository : IFavoriteItemRepository
    {
        private readonly Context _context;
        public FavoriteItemRepository(Context context)
        {
            _context = context;
        }
        public int AddFavoriteItem(FavoriteItem favoriteItem)
        {
            _context.FavoriteItems.Add(favoriteItem);
            _context.SaveChanges();
            return favoriteItem.Id;
        }

        public void DeleteFavoriteItem(int favoriteItemId)
        {
            var result = _context.FavoriteItems.Find(favoriteItemId);
            if (result != null)
            {
                _context.FavoriteItems.Remove(result);
                _context.SaveChanges();
            }
        }

        public IQueryable<FavoriteItem> GetAllFavoriteItems()
        {
            var result = _context.FavoriteItems;
            return result;
        }

        public void UpdateFavoriteItem(FavoriteItem favoriteItems)
        {
            _context.FavoriteItems.Update(favoriteItems);
            _context.SaveChanges();
        }
    }
}
