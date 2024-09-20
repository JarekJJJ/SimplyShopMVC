using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Application.ViewModels.Message
{
    public class TicketMessageGroupForListVm
    {
        public MessageTicketForListVm messageTicketForListVm { get; set; }
        public List<MessageForListVm> messageForListVm { get; set;}
    }
}
