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
        IndexListVm GetItemsToIndex(int quantityItem);
        ListItemShopIndexVm GetAllCategories();
        ListItemShopIndexVm GetItemsByCategory(int categoryId);


    }
}
