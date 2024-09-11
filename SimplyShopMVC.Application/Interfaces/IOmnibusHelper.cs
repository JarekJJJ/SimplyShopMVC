using SimplyShopMVC.Application.ViewModels.Item;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Application.Interfaces
{
    public interface IOmnibusHelper
    {
        void AddPriceToHistory(decimal priceN, string eanCode, int warehouseId);
        void AddPriceToHistory(ItemWarehouseForListVm item);
        List<OmnibusPriceToListVm> GetOmnibusPrice(string eanCode);
        List<OmnibusPriceToListVm> GetOmnibusPrice(ItemForListVm item);
        List<OmnibusPriceToListVm> GetAllOmnibusPrice(string eanCode, int numberMonths);
    }
}
