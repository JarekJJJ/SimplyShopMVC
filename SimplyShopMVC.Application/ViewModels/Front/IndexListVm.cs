using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Application.ViewModels.Front
{
    public class IndexListVm
    {
        public List<FrontItemForList> frontItemForLists { get; set; }
        public List<FrontItemForList> frontItemNews { get; set; }
        public string userId { get; set; }
    }
}
