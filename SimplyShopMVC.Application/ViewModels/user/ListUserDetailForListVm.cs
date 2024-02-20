using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Application.ViewModels.user
{
    public class ListUserDetailForListVm
    {
        public List<UserDetailForListVm> listUserDetail { get; set; }
        public UserDetailForListVm userDetail { get; set; }
    }
}
