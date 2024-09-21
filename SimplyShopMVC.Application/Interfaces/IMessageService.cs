using SimplyShopMVC.Application.ViewModels.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Application.Interfaces;

public interface IMessageService
{
    bool SendMessage(MessageForListVm message);
    List<TicketMessageGroupForListVm> GetMessagesForUser(string userId);
    List<TicketMessageGroupForListVm> GetMessageForAdmin();
    MessageForListVm GetMessageDetail(int messageId, bool isAdmin, string? userId);
    bool SendMessageRe(MessageForListVm message);
    bool SendMessageUserRe(MessageForListVm message);

}
