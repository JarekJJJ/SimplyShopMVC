using SimplyShopMVC.Domain.Interface;
using SimplyShopMVC.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Infrastructure.Repositories
{
    public class ItemRepository: IItemRepository
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
    }
}
