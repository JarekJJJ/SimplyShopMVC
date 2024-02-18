using SimplyShopMVC.Application.ViewModels.Front;
using SimplyShopMVC.Application.ViewModels.Item;
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
    }
}
