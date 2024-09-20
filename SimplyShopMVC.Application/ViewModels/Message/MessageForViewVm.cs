using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Application.ViewModels.Message
{
    public class MessageForViewVm
    {
        [ValidateNever]
        public MessageForListVm messageForViewVm { get; set; }      
        public MessageForListVm messageForSend { get; set; }
        public int mainMessageId { get; set; }
    }
}
