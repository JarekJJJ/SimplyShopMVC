using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Application.ViewModels.Order
{
    public class ListDeliveryForListVm
    {
        public List<DeliveryForListVm> listDelivery { get; set; }
        public  DeliveryForListVm delivery { get; set; }
        public int options { get; set; }

    }
}
