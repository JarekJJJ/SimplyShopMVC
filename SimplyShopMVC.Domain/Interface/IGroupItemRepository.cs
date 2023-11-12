using SimplyShopMVC.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Domain.Interface
{
    public interface IGroupItemRepository
    {
        int AddGroupItem(GroupItem groupItem);
        void UpdateGroupItem(GroupItem groupItem);
        IQueryable<GroupItem> GetAllGroupItem();
        void DeleteGroupItem(int groupItemId);
    }
}
