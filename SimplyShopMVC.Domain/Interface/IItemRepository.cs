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
        void DeleteItem(int Itemid);
        
        int AddItem(Item item);
   
        IQueryable<Item> GetItemsByCategoryId(int categoryId);
       
        Item GetItemById(int id);
     
        IQueryable<Item> GetAllItems();
      
    }
}
