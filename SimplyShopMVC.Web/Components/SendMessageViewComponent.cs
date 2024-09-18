using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SimplyShopMVC.Application.Interfaces;
using SimplyShopMVC.Application.ViewModels.Message;

namespace SimplyShopMVC.Web.Components
{
    public class SendMessageViewComponent : ViewComponent
    {
        private readonly IMessageService _messageService;
        public SendMessageViewComponent(IMessageService messageService)
        {
            _messageService = messageService;

        }
        [HttpGet]
        public async Task<IViewComponentResult> InvokeAsync()
        {
            MessageForListVm message = new MessageForListVm();
            return View(message);
        }
    }
}
