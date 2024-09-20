using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Application.ViewModels.Message
{
    public class ListTicketMessageGroupForListVm
    {
        public List<TicketMessageGroupForListVm> listTicketMessageGroup { get; set; }
        public MessageForListVm messageForListVm { get; set; }
    }
}
