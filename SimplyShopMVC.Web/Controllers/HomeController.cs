using Microsoft.AspNetCore.Mvc;
using SimplyShopMVC.Domain.Interface;
using SimplyShopMVC.Web.Models;
using SimplyShopMVC.Application.Interfaces;
using System.Diagnostics;
using SimplyShopMVC.Application.ViewModels.Item;
using SimplyShopMVC.Application.ViewModels.Front;
using SimplyShopMVC.Application.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using AspNetCore.ReCaptcha;
using SimplyShopMVC.Application.ViewModels.Message;

namespace SimplyShopMVC.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IFrontService _frontService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly IArticleService _articleService;
        private readonly IMessageService _messageService;

        public HomeController(ILogger<HomeController> logger, IFrontService frontService, UserManager<IdentityUser> userManager, IEmailService emailService, IArticleService articleService, IMessageService messageService)
        {
            _logger = logger;
            _frontService = frontService;
            _userManager = userManager;
            _emailService = emailService;
            _articleService = articleService;
            _messageService = messageService;
        }

        public IActionResult Index()
        {
            IndexListVm listItems = new IndexListVm();
            var tempDataContent = TempData["Message"] as string[];
            if(tempDataContent != null)
            {
                listItems.errorMessage = tempDataContent?.ToList();
               
            }

         
            var panel1Text = _articleService.GetArticleDetailsByTag("indexPanel1");
            if(panel1Text != null)
            {
                listItems.articlePanel1 = panel1Text;
            }
            var panel2Text = _articleService.GetArticleDetailsByTag("indexPanel2");
            if (panel1Text != null)
            {
                listItems.articlePanel2 = panel2Text;
            }
            var panel3Text = _articleService.GetArticleDetailsByTag("indexPanel3");
            if (panel1Text != null)
            {
                listItems.articlePanel3 = panel3Text;
            }
            //var iduser = _userManager.GetUserId(User);
            //listItems.userId = iduser; //test do usunięcia
            return View(listItems);

        }

        public IActionResult Privacy()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Contact()
        {
            return View(new MessageForListVm());
        }
        [ValidateReCaptcha]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contact(MessageForListVm message)
        {
            if (ModelState.IsValid)
            {
                var iduser = _userManager.GetUserId(User);
                if (iduser != null)
                {
                    message.SenderUserId = iduser;
                }
                else
                {
                    message.SenderUserId = string.Empty;
                }
                var result = _messageService.SendMessage(message);
                if (result)
                {
                    TempData["Success"] = "Pomyślnie wysłano wiadomość";
                }
                else
                {
                    TempData["Error"] = "Nie udało się wysłać wiadomości";
                }
                return RedirectToAction("Contact");
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                ModelState.AddModelError("AntySpamResult", "Wypełnij pole reCaptcha");
                errors.Insert(0, "Nie udało się wysłać wiadomości");
                errors.Insert(1, "Wystąpiły błędy:");
                TempData["Message"] = errors;
            }


            return View(message);

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
