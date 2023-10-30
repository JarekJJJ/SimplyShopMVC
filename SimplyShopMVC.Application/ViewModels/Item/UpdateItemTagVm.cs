using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Application.ViewModels.Item
{
    public class UpdateItemTagVm
    {
        public List<ItemTagsForListVm> itemTags { get; set; }
        public ItemTagsForListVm itemTag { get; set; }
        public List<CountItemTagVm> countTag { get; set; }
        public string searchTag { get; set; }
    }
}
