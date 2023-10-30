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
        int AddCategory(AddItemVm item);
        UpdateItemVm GetItemToUpdate(int itemId);
        void AddItemWarehouse(AddItemWarehouseVm model);
        AddItemWarehouseVm ListItemToUpdate(string searchItem);
        AddItemWarehouseVm AddToUpdateItemWarehouse(int itemId);
        AddItemVm AddItemToUpdate(int selectedItem);
        //ItemTag
        UpdateItemTagVm ListItemTagToUpdate(string? searchTag);
        UpdateItemTagVm GetItemTagToUpdate(int tagId);
        void UpdateItemTag(UpdateItemTagVm itemTag, int options);
        int AddItemTag(AddItemVm item);

        //ItemDetailVm GetItemDetails(int itemId);
        //ListItemForListVm GetAllItemsByTagId(int tagId);
        //ListItemForListVm GetAllItemsForList();
    }
}





