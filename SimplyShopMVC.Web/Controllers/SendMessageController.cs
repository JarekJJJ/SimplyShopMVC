using AspNetCore.ReCaptcha;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using SimplyShopMVC.Application.Interfaces;
using SimplyShopMVC.Application.ViewModels.Message;

namespace SimplyShopMVC.Web.Controllers
{
    public class SendMessageController : Controller
    {
        private readonly IMessageService _messageService;
        private readonly UserManager<IdentityUser> _userManager;
        public SendMessageController(IMessageService messageService, UserManager<IdentityUser> userManager)
        {
            _messageService = messageService;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            
            return View();
        }    
    }
}
