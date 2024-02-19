using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Application.ViewModels.Item
{
    public class ListGroupItemForListVm
    {
        public List<GroupItemForListVm> ListGroupItem { get; set; }
        public GroupItemForListVm GroupItem { get; set; }
        public int options { get; set; }
    }
}
