using AutoMapper;
using AutoMapper.QueryableExtensions;
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
    private readonly IUserRepository _userRepo;
    public MessageService(IMessageRepository messageRepository, IMapper mapper, IEmailService emailService, IUserRepository userRepository)
    {
        _messageRepo = messageRepository;
        _mapper = mapper;
        _emailService = emailService;
        _userRepo = userRepository;
    }
    public List<TicketMessageGroupForListVm> GetMessageForAdmin()
    {
        var listMessage = _messageRepo.GetAllMessage().ProjectTo<MessageForListVm>(_mapper.ConfigurationProvider).ToList();
        var listTicketMessage = _messageRepo.GetAllMessageTicket().ProjectTo<MessageTicketForListVm>(_mapper.ConfigurationProvider).ToList();
       
        var groupMesageByTicket = listMessage.OrderByDescending(d => d.CreatedDate)
            .GroupBy(t => t.MessageTicketId)
            .Select(g => new TicketMessageGroupForListVm
            {
                messageTicketForListVm = listTicketMessage.First(t => t.Id == g.Key),
                messageForListVm = g.ToList()
            }).ToList();

        return groupMesageByTicket;
    }

    public List<TicketMessageGroupForListVm> GetMessagesForUser(string userId)
    {
        List<MessageTicketForListVm> listTicketMessage = new List<MessageTicketForListVm>();
        var listMessage = _messageRepo.GetAllMessage().Where(u=>u.SenderUserId == userId).ProjectTo<MessageForListVm>(_mapper.ConfigurationProvider).ToList();
        var listAllTicketMessage = _messageRepo.GetAllMessageTicket().ToList();
        if (listMessage.Any())
        {
            foreach (var message in listMessage)
            {
                var ticketMessage = listAllTicketMessage.FirstOrDefault(m => m.Id == message.MessageTicketId);
                if (ticketMessage != null)
                {
                    if (!listTicketMessage.Any(m => m.Id == ticketMessage.Id))
                    {
                        listTicketMessage.Add(_mapper.Map<MessageTicketForListVm>(ticketMessage));
                    }
                }
            }
            var groupMesageByTicket = listMessage.OrderByDescending(d => d.CreatedDate)
           .GroupBy(t => t.MessageTicketId)
           .Select(g => new TicketMessageGroupForListVm
           {
               messageTicketForListVm = listTicketMessage.First(t => t.Id == g.Key),
               messageForListVm = g.ToList()
           }).ToList();
            return groupMesageByTicket;
        }
        return new List<TicketMessageGroupForListVm>();

    }

    public bool SendMessage(MessageForListVm message)
    {
        if (message != null)
        {
            message.CreatedDate = DateTime.Now;
            message.StatusMessage = StatusMessage.Delivered;
            Random random = new Random();
            int randomNumber = random.Next(1, 1000);
            string ticket = DateTime.Now.ToString("yyyyMMddHHmmss") + randomNumber.ToString();
            var resultTicket = GetOrCreateMessageTicket(ticket);
            if (resultTicket != null)
            {
                message.TicketMessage = resultTicket.Name;
                message.MessageTicketId = resultTicket.Id;
                var id = _messageRepo.AddMessage(_mapper.Map<Message>(message));

                if (id > 0)
                {
                    _emailService.SendContactEmail(message.SenderAddress, message.Title, message.Body, message.TicketMessage);
                    _emailService.SendEmail(message.SenderAddress, "Ticket: #" + message.TicketMessage + "#",
                        "Dziękujemy za przesłanie wiadomości. <br>" +
                        " Odpowiemy najszybciej jak to możliwe. <br>" +
                        "Z poważaniem zespół HKR24.PL");
                    return true;
                }
            }

        }
        return false;
    }
    public bool SendMessageRe(MessageForListVm message) // Wysyłka wiadomości przez administracje do klienta
    {
        MessageTicket resultTicket = new MessageTicket();
        if (message != null)
        {
            message.CreatedDate = DateTime.Now;
            message.StatusMessage = StatusMessage.Replied;
            if (String.IsNullOrEmpty(message.TicketMessage))
            {
                Random random = new Random();
                int randomNumber = random.Next(1, 1000);
                string ticket = DateTime.Now.ToString("yyyyMMddHHmmss") + randomNumber.ToString();
                resultTicket = GetOrCreateMessageTicket(ticket);
            }
            else
            {
                resultTicket = GetOrCreateMessageTicket(message.TicketMessage);
            }


            if (resultTicket != null)
            {
                message.TicketMessage = resultTicket.Name;
                message.MessageTicketId = resultTicket.Id;
                var id = _messageRepo.AddMessage(_mapper.Map<Message>(message));

                if (id > 0)
                {
                    // _emailService.SendContactEmail(message.SenderAddress, message.Title, message.Body, message.TicketMessage);
                    _emailService.SendEmail(message.SenderAddress, message.Title + " Ticket: #" + message.TicketMessage + "#", message.Body);
                    return true;
                }
            }

        }
        return false;
    }
    public bool SendMessageUserRe(MessageForListVm message) // Wysyłka wiadomości przez klienta do Administracji
    {
        MessageTicket resultTicket = new MessageTicket();
        if (message != null)
        {
            message.CreatedDate = DateTime.Now;
            message.StatusMessage = StatusMessage.Replied;
            if (String.IsNullOrEmpty(message.TicketMessage))
            {
                Random random = new Random();
                int randomNumber = random.Next(1, 1000);
                string ticket = DateTime.Now.ToString("yyyyMMddHHmmss") + randomNumber.ToString();
                resultTicket = GetOrCreateMessageTicket(ticket);
            }
            else
            {
                resultTicket = GetOrCreateMessageTicket(message.TicketMessage);
            }


            if (resultTicket != null)
            {
                message.TicketMessage = resultTicket.Name;
                message.MessageTicketId = resultTicket.Id;
                var id = _messageRepo.AddMessage(_mapper.Map<Message>(message));

                if (id > 0)
                {
                    _emailService.SendContactEmail(message.SenderAddress, message.Title, message.Body, message.TicketMessage);
                    //_emailService.SendEmail(message.SenderAddress, message.Title + " Ticket: #" + message.TicketMessage + "#", message.Body);
                    return true;
                }
            }

        }
        return false;
    }
    public MessageTicket GetOrCreateMessageTicket(string ticketName)
    {
        MessageTicket ticket = new MessageTicket();
        if (!String.IsNullOrEmpty(ticketName))
        {
            var result = _messageRepo.GetAllMessageTicket().Where(n => n.Name == ticketName).FirstOrDefault();
            if (result == null)
            {

                ticket.Name = ticketName;
                ticket.StatusTicket = StatusTicket.NewTicket;
                ticket.IsDeleted = false;
                _messageRepo.AddMessageTicket(ticket);
                result = ticket;
            }
            return result;
        }
        return ticket;
    }
    public MessageForListVm GetMessageDetail(int messageId, bool isAdmin, string? userId)
    {
        var messageDetail = _mapper.Map<MessageForListVm>(_messageRepo.GetMessage(messageId));
        bool userAccess = false;
        if (messageDetail != null)
        {
            if (!String.IsNullOrEmpty(messageDetail.SenderUserId))
            {
                var userSenderDetail = _userRepo.GetAllUsers().FirstOrDefault(u => u.UserId == messageDetail.SenderUserId);
                if (userSenderDetail != null)
                {
                    messageDetail.senderTrueEmail = userSenderDetail.EmailAddress;
                }
            }
            if (!String.IsNullOrEmpty(userId))
            {
                var userDetail = _userRepo.GetAllUsers().Where(u => u.UserId == userId).FirstOrDefault();
                if (messageDetail != null)
                {
                    if (userDetail != null)
                    {
                        if (messageDetail.SenderUserId == userId)
                        {
                            userAccess = true;
                        }

                    }

                }

            }
            if (isAdmin || userAccess)
            {
                return messageDetail;
            }
        }

        return new MessageForListVm();
    }

}
