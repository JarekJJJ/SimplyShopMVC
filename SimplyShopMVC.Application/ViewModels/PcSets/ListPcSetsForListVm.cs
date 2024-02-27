using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Application.ViewModels.PcSets
{
    public class ListPcSetsForListVm
    {
        public List<PcSetsForListVm> listSets { get; set; }
        public PcSetsForListVm pcSet { get; set; }
        public List<SetsItemForListVm> setsItems { get; set; }
        public SetsItemForListVm setItem { get; set; }
    }
}
