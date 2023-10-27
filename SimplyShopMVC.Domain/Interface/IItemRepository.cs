using SimplyShopMVC.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Domain.Interface
{
    public interface IItemRepository
    {
        //podstawowy crud
        void DeleteItem(int itemid);
        void UpdateItem(Item item);
        int AddItem(Item item);
        IQueryable<Item> GetAllItems();
        Item GetItemById(int id);
        //Tagi
        IQueryable<ItemTag> GetAllItemTags();
        IQueryable<Item> GetItemsByTagId(int tagId);
        IQueryable<ConnectItemTag> GetConnectItemTags(int articleId);
        int AddItemTag(ItemTag itemTag);
        ItemTag GetItemTagByTagId(int tagId);
        void AddConnectionItemTags(int itemId, ItemTag tags);
        void DeleteConnectionItemTags(int itemId);
        // Kategorie
        IQueryable<Category> GetAllCategories();
        IQueryable<Item> GetItemsByCategoryId(int categoryId);  
        int AddCategory(Category category);
        Category GetCategoryById(int id);
        void UpdateCategory(Category category);
        void DeleteCategory(int categoryId);
        //Warehouse
        int AddWarehouse(Warehouse warehouse);
        IQueryable<Warehouse> GetAllWarehouses();
        void DeleteWarehouse(int warehouseId);
        // ItemWarehouse
        int AddItemWarehouse(ItemWarehouse itemWarehouse);
        void UpdateItemWarehouse(ItemWarehouse itemWarehouse);
        IQueryable<ItemWarehouse> GetAllItemWarehouses();
        // Vat Rate
        IQueryable<VatRate> GetAllVatRate();
















    }
}
