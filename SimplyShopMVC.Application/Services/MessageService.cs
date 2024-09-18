using SimplyShopMVC.Application.Interfaces;
using SimplyShopMVC.Application.ViewModels.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Application.Services
{
    public class MessageService : IMessageService
    {
        public List<MessageForListVm> GetMessageForAdmin()
        {
            throw new NotImplementedException();
        }

        public List<MessageForListVm> GetMessagesForUser(string userId)
        {
            throw new NotImplementedException();
        }

        public bool SendMessage(MessageForListVm message)
        {
            return true;
        }
    }
}
