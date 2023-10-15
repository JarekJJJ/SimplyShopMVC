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
        AddItemVm AddItem(AddItemVm item);
        void UpdateItem(UpdateItemVm item, List<string> selectedImage);      
        void DeleteItem(int id);
        int AddItemTag(AddItemVm item);
        UpdateItemVm GetItemToUpdate(int itemId);
        //ItemDetailVm GetItemDetails(int itemId);
        //ListItemForListVm GetAllItemsByTagId(int tagId);
        //ListItemForListVm GetAllItemsForList();
    }
}





