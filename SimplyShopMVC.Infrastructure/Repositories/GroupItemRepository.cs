using SimplyShopMVC.Domain.Interface;
using SimplyShopMVC.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Infrastructure.Repositories
{
    public class GroupItemRepository : IGroupItemRepository
    {
        private readonly Context _context;
        public GroupItemRepository(Context context)
        {
            _context = context;
        }
        public int AddGroupItem(GroupItem groupItem)
        {
            _context.Add(groupItem);
            _context.SaveChanges();
            return groupItem.Id;
        }

        public void DeleteGroupItem(int groupItemId)
        {
           var result = _context.GroupItems.FirstOrDefault(i=>i.Id== groupItemId);
            if (result != null)
            {
                _context.Remove(result);
                _context.SaveChanges();
            }
        }

        public IQueryable<GroupItem> GetAllGroupItem()
        {
            var result = _context.GroupItems;
            return result;
        }

        public void UpdateGroupItem(GroupItem groupItem)
        {
           _context.Update(groupItem);
            _context.SaveChanges();
        }
    }
}
