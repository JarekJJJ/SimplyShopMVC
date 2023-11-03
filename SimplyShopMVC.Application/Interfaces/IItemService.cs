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
        //Category
        int AddCategory(AddItemVm item);
        UpdateCategoryVm ListCategoryToUpdate(string? searchCategory);
        UpdateCategoryVm GetCategoryToUpdate(int categoryId);
        void DeleteCategory(int categoryId);
        void UpdateCategory(UpdateCategoryVm category, int options);

        //ItemDetailVm GetItemDetails(int itemId);
        //ListItemForListVm GetAllItemsByTagId(int tagId);
        //ListItemForListVm GetAllItemsForList();
    }
}





