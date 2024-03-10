using SimplyShopMVC.Application.ViewModels.Item;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Application.ViewModels.PcSets
{
    public class PcSetDetailVm
    {
        public PcSetsForListVm pcSet { get; set; }
        public List<SetsItemForListVm> listSetItem { get; set; }
        public List<ItemForListVm> listItem { get; set; }
    }
}
