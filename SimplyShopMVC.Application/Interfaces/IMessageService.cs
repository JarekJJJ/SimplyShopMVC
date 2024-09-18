using SimplyShopMVC.Application.ViewModels.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Application.Interfaces;

public interface IMessageService
{
    public bool SendMessage(MessageForListVm message);
    public List<MessageForListVm> GetMessagesForUser(string userId);
    public List<MessageForListVm> GetMessageForAdmin();
}
