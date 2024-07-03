using SimplyShopMVC.Application.ViewModels.Front;
using SimplyShopMVC.Application.ViewModels.Item;
using SimplyShopMVC.Application.ViewModels.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Domain.Interface
{
    public interface IFrontService
    {
        FrontItemForList GetItemDetail(int id, string userId);
        List<FrontItemForList> GetItemsToIndex(int quantityItem, string tagName, string userId);
        ListItemShopIndexVm GetAllCategories(string userId);
        ListItemShopIndexVm GetItemsByCategory(int categoryId, int pageSize, int pageNo, string searchString, int selectedTags, string userId);
        CartForListVm GetCart(string userId);
        int AddFavoriteItemToList(int itemId, string userId);
        ListFavoriteItemsForListVm GetAllFavoriteItems(string userId);
        int DeleteFavoriteItemFromList(int favoriteItemId);
    }
}
