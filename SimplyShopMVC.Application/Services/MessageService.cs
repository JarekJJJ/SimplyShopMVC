using AutoMapper;
using SimplyShopMVC.Application.Interfaces;
using SimplyShopMVC.Application.ViewModels.Message;
using SimplyShopMVC.Domain.Interface;
using SimplyShopMVC.Domain.Model.Messages;
using SimplyShopMVC.Domain.Model.Messages.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Application.Services;

public class MessageService : IMessageService
{
    private readonly IMessageRepository _messageRepo;
    private readonly IMapper _mapper;
    private readonly IEmailService _emailService;
    public MessageService(IMessageRepository messageRepository, IMapper mapper, IEmailService emailService)
    {
        _messageRepo = messageRepository;
        _mapper = mapper;
        _emailService = emailService;
    }
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
        if(message!=null)
        {
            message.CreatedDate = DateTime.Now;
            message.StatusMessage = StatusMessage.Delivered;
           var id = _messageRepo.AddMessage(_mapper.Map<Message>(message));
           
            if (id > 0)
            {
                var editMesage = _messageRepo.GetMessage(id);
            editMesage.TicketMessage = DateTime.Now.ToString("yyyyMMddHHmmss") + id.ToString();
                _messageRepo.UpdateMessage(editMesage);
                _emailService.SendContactEmail(message.SenderAddress, message.Title, message.Body, editMesage.TicketMessage);
                _emailService.SendEmail(message.SenderAddress, "Ticket: #" + editMesage.TicketMessage + "#",
                    "Dziękujemy za przesłanie wiadomości. <br>" +
                    " Odpowiemy najszybciej jak to możliwe. <br>" +
                    "Z poważaniem zespół HKR24.PL");
                return true;
            }
        }
        return false;
    }
}
