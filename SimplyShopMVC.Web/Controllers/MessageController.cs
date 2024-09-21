using AspNetCore.ReCaptcha;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using SimplyShopMVC.Application.Interfaces;
using SimplyShopMVC.Application.ViewModels.Message;


namespace SimplyShopMVC.Web.Controllers
{
    [Authorize]
    public class MessageController : Controller
    {
        private readonly IMessageService _messageService;
        private readonly UserManager<IdentityUser> _userManager;
        public MessageController(IMessageService messageService, UserManager<IdentityUser> userManager)
        {
            _messageService = messageService;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            
            return View();
        }
        [Authorize(Roles = "Admin")]
        public IActionResult AdminListMessage()
        {
            ListTicketMessageGroupForListVm listTicketMessageGroup = new ListTicketMessageGroupForListVm();
            var listMessage = _messageService.GetMessageForAdmin();
            listTicketMessageGroup.listTicketMessageGroup = listMessage;
            listTicketMessageGroup.messageForListVm = new MessageForListVm();
            return View(listTicketMessageGroup);
        }
        [Authorize]
        public IActionResult UserListMessage()
        {
            ListTicketMessageGroupForListVm listTicketMessageGroup = new ListTicketMessageGroupForListVm();
            var userId = _userManager.GetUserId(User);
            if(userId != null)
            {
                var listMessage = _messageService.GetMessagesForUser(userId);
                listTicketMessageGroup.listTicketMessageGroup = listMessage;
            }       
            listTicketMessageGroup.messageForListVm = new MessageForListVm();
            return View(listTicketMessageGroup);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult DetailMessageAdmin(int Id)
        {
           
            var message = _messageService.GetMessageDetail(Id, true,"");
            MessageForViewVm messageForView= new MessageForViewVm();
            messageForView.messageForViewVm = message;
            messageForView.messageForSend = new MessageForListVm();
            return View(messageForView);
        }
        [Authorize]
        public IActionResult DetailMessageUser(int Id)
        {
            MessageForViewVm messageForView = new MessageForViewVm();
            var userId = _userManager.GetUserId(User);
            if (userId != null)
            {
                var message = _messageService.GetMessageDetail(Id, false, userId);
                messageForView.messageForViewVm = message;
            }                                    
            messageForView.messageForSend = new MessageForListVm();
            return View(messageForView);
        }
        [ValidateReCaptcha]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendMessageRe(MessageForViewVm message)
        {
            if(message.mainMessageId > 0)
            {
                var messageView = _messageService.GetMessageDetail(message.mainMessageId, true, "");
                if (messageView != null)
                {
                    message.messageForViewVm = messageView;
                }
            }
            if (ModelState.IsValid)
            {
                var iduser = _userManager.GetUserId(User);
                if (String.IsNullOrEmpty(message.messageForSend.SenderUserId))
                {
                    if (iduser != null)
                    {
                        message.messageForSend.SenderUserId = iduser;
                    }
                    else
                    {
                        message.messageForSend.SenderUserId = string.Empty;
                    }
                }
           
                var remoteIpAddress = HttpContext.Connection.RemoteIpAddress;
                string userIpAddress = remoteIpAddress != null ? remoteIpAddress.ToString() : "IP not found";
                message.messageForSend.SenderIpAddress = userIpAddress;
                var result =  _messageService.SendMessageRe(message.messageForSend);
                if (result)
                {
                    TempData["Success"] = "Pomyślnie wysłano wiadomość";
                }
                else
                {
                    TempData["Error"] = "Nie udało się wysłać wiadomości";
                }
                return RedirectToAction("AdminListMessage");
            }
            else
            {
                ModelState.AddModelError("AntySpamResult", "Wypełnij pole reCaptcha");
                TempData["Error"] = "Nie udało się wysłać wiadomości - Wystąpiły błędy. ";
            }


            return View("DetailMessageAdmin",message);

        }
        [ValidateReCaptcha]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendMessage(MessageForViewVm message)
        {
            if (message.mainMessageId > 0)
            {
                var messageView = _messageService.GetMessageDetail(message.mainMessageId, true, "");
                if (messageView != null)
                {
                    message.messageForViewVm = messageView;
                }
            }
            if (ModelState.IsValid)
            {
                var iduser = _userManager.GetUserId(User);
                if (iduser != null)
                {
                    message.messageForSend.SenderUserId = iduser;
                }
                else
                {
                    message.messageForSend.SenderUserId = string.Empty;
                }
                var remoteIpAddress = HttpContext.Connection.RemoteIpAddress;
                string userIpAddress = remoteIpAddress != null ? remoteIpAddress.ToString() : "IP not found";
                message.messageForSend.SenderIpAddress = userIpAddress;
                var result = _messageService.SendMessageUserRe(message.messageForSend);
                if (result)
                {
                    TempData["Success"] = "Pomyślnie wysłano wiadomość";
                }
                else
                {
                    TempData["Error"] = "Nie udało się wysłać wiadomości";
                }
                return RedirectToAction("UserListMessage");
            }
            else
            {
                ModelState.AddModelError("AntySpamResult", "Wypełnij pole reCaptcha");
                TempData["Error"] = "Nie udało się wysłać wiadomości - Wystąpiły błędy. ";
            }


            return View("DetailMessageUser", message);

        }
    }
}
