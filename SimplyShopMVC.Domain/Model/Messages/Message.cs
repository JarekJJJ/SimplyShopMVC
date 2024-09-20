using SimplyShopMVC.Domain.Model.Messages.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Domain.Model.Messages;

public class Message
{
    public int Id { get; set; }
    public string SenderAddress { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public string? SenderUserId { get; set; }
    public string TicketMessage { get; set; } = string.Empty;
    public bool IsDeleted { get; set; } = false;
    public int MessageTicketId { get; set; }
    public DateTime? CreatedDate { get; set; }
    public DateTime UpdateDateTime { get; set; } = DateTime.Now;
    public StatusMessage StatusMessage { get; set; }
    public string? SenderIpAddress { get; set; } = string.Empty;
    public bool PrivacyPolicy { get; set; }
    public virtual MessageTicket MessageTicket { get; set; }
}
