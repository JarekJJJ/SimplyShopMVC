using Microsoft.EntityFrameworkCore;
using SimplyShopMVC.Domain.Interface;
using SimplyShopMVC.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Infrastructure.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly Context _context;
        public ItemRepository(Context context)
        {
            _context = context;
        }

        public void DeleteItem(int Itemid)
        {
            var item = _context.Items.Find(Itemid);
            if (item != null)
            {
                _context.Items.Remove(item);
                _context.SaveChanges();
            }
        }
        public int AddItem(Item item)
        {
            _context.Items.Add(item);
            _context.SaveChanges();
            return item.Id;
        }
        public IQueryable<Item> GetItemsByCategoryId(int categoryId)
        {
            var items = _context.Items.Where(i => i.CategoryId == categoryId);
            return items;
        }
        public Item GetItemById(int id)
        {
            var item = _context.Items.FirstOrDefault(i => i.Id == id);

            return item;

        }
        public IQueryable<Item> GetAllItems()
        {
            var items = _context.Items;
            return items;
        }

        public void UpdateItem(Item item)
        {        
               _context.Items.Update(item);
                _context.SaveChanges();          
        }

        public IQueryable<ItemTag> GetAllItemTags()
        {
            var result = _context.ItemTags;
            return result;
        }

        public IQueryable<Item> GetItemsByTagId(int tagId)
        {
            var result = _context.Items.Where(at => at.ConnectItemTags.Any(at => at.ItemTagId == tagId));
            return result;
        }

        public IQueryable<ConnectItemTag> GetConnectItemTags(int itemId)
        {
            var result = _context.ConnectItemTag.Where(t => t.ItemId == itemId);
            return result;
        }

        public int AddItemTag(ItemTag itemTag)
        {
            _context.ItemTags.Add(itemTag);
            _context.SaveChanges();
            return itemTag.Id;
        }

        public ItemTag GetItemTagByTagId(int tagId)
        {
            var result = _context.ItemTags.FirstOrDefault(t => t.Id == tagId);
            return result;
        }

        public void AddConnectionItemTags(int itemId, ItemTag tags)
        {
            var item = GetItemById(itemId);
            ConnectItemTag con = new ConnectItemTag();
            con.ItemId = item.Id;
            con.ItemTagId = tags.Id;
            _context.ConnectItemTag.Add(con);
            _context.SaveChanges();
        }
        public void UpdateItemTag(ItemTag itemTag)
        {
           _context.ItemTags.Update(itemTag);
            _context.SaveChanges();
        }

        public void DeleteConnectionItemTags(int itemId)
        {
            var result = _context.ConnectItemTag.Where(a => a.ItemId == itemId);
            _context.ConnectItemTag.RemoveRange(result);
            _context.SaveChanges();
        }
        public void DeleteItemTag(int itemTagId)
        {
            var result = _context.ItemTags.Find(itemTagId);
            if(result!=null)
            {
                _context.ItemTags.Remove(result);
                _context.SaveChanges();
            }
        }

        public IQueryable<Category> GetAllCategories()
        {
            var categories = _context.Categories;
            return categories;
        }

        public int AddCategory(Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
            return category.Id;
        }

        public Category GetCategoryById(int id)
        {
            var result = _context.Categories.FirstOrDefault(c => c.Id == id);
            return result;
        }

        public void UpdateCategory(Category category)
        {         
                _context.Categories.Update(category);
                _context.SaveChanges();

        }

        public void DeleteCategory(int categoryId)
        {
            var result = _context.Categories.FirstOrDefault(c => c.Id == categoryId);
            if (result != null)
            {
                _context.Categories.Remove(result);
                _context.SaveChanges();
            }
        }

        public int AddWarehouse(Warehouse warehouse)
        {
            _context.Warehouses.Add(warehouse);
            _context.SaveChanges();
            return warehouse.Id;
        }

        public void DeleteWarehouse(int warehouseId)
        {
            var result = _context.Warehouses.FirstOrDefault(w => w.Id == warehouseId);
            if (result != null)
            {
                _context.Warehouses.Remove(result);
                _context.SaveChanges();
            }
        }

        public int AddItemWarehouse(ItemWarehouse itemWarehouse)
        {
            _context.ItemWarehouses.Add(itemWarehouse);
            _context.SaveChanges();
            return itemWarehouse.Id;
        }
        public IQueryable<ItemWarehouse> GetAllItemWarehouses()
        {
            var result = _context.ItemWarehouses;
            return result;
        }

        public IQueryable<Warehouse> GetAllWarehouses()
        {
            var result = _context.Warehouses;
            return result;
        }

        public void UpdateItemWarehouse(ItemWarehouse itemWarehouse)
        {
            var itemToUpdate = _context.ItemWarehouses.FirstOrDefault(i=>i.ItemId==itemWarehouse.ItemId || i.WarehouseId==itemWarehouse.WarehouseId);
            if (itemToUpdate != null)
            {
                itemWarehouse.Id = itemToUpdate.Id;
                _context.Entry(itemToUpdate).State = EntityState.Detached;
                _context.Update(itemWarehouse);
                _context.SaveChanges();
            }                 
        }
        public IQueryable<VatRate> GetAllVatRate()
        {
            var result = _context.VatRates;
            return result;
        }    
    }
}
