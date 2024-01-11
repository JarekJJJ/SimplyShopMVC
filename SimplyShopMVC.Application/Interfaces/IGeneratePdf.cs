using SimplyShopMVC.Application.ViewModels.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Application.Interfaces
{
    public interface IGeneratePdf
    {
        byte[] GenertateOrderPdf(OrderFromCartVm orderFromCart);
    }
}
