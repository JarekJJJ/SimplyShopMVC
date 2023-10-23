using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using SimplyShopMVC.Application.ViewModels.Item;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Application.Interfaces
{
    public interface IItemService
    {
        AddItemVm AddItem(AddItemVm item, IWebHostEnvironment webHostFolder);
        int UpdateItem(AddItemVm item);      
        void DeleteItem(int id);
        int AddItemTag(AddItemVm item);
        int AddCategory(AddItemVm item);
        UpdateItemVm GetItemToUpdate(int itemId);
        AddItemWarehouseVm AddItemWarehouse(AddItemWarehouseVm item);
        AddItemWarehouseVm ListItemToUpdate(string searchItem);

        AddItemVm AddItemToUpdate(int selectedItem);
        //ItemDetailVm GetItemDetails(int itemId);
        //ListItemForListVm GetAllItemsByTagId(int tagId);
        //ListItemForListVm GetAllItemsForList();
    }
}





