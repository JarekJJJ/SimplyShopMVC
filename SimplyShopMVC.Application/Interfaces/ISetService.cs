using SimplyShopMVC.Application.ViewModels.Front;
using SimplyShopMVC.Application.ViewModels.PcSets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Application.Interfaces
{
    public interface ISetService
    {
        ListPcSetsForListVm SetHandling(ListItemShopIndexVm result, int options);

    }
}
