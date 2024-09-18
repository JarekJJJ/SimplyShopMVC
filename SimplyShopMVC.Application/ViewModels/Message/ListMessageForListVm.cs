using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Application.ViewModels.Message
{
    public class ListMessageForListVm
    {
        public List<MessageForListVm> listMessages { get; set; }
        public MessageForListVm message { get; set; }
    }
}
