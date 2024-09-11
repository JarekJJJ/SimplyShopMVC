using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Application.Interfaces
{
    public interface IPriceCalculate
    {
        decimal priceCalc(int itemId, int warehouseId, string userId);
        decimal priceCalc(int itemId, int groupItemId, int warehouseId, string userId);
        decimal omnibusPriceCalc(int itemId, decimal netPurchasePrice, string userId);
    }
}
