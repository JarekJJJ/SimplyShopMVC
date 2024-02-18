using Humanizer;
using Microsoft.AspNetCore.Identity.UI.Services;
using SimplyShopMVC.Domain.Interface;
using System.Net.Mail;
using System.Net.Mime;
using System.Net;

namespace SimplyShopMVC.Web.Services
{
    public class EmailSenderService : IEmailSender
    {
        private readonly IEmailConfiguration _emailConfiguration;
        public EmailSenderService(IEmailConfiguration emailConfiguration)
        {
            _emailConfiguration = emailConfiguration;

        }
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            using (SmtpClient smtpClient = new SmtpClient(_emailConfiguration.SmtpServer, _emailConfiguration.SmtpPort))
            {
                smtpClient.Credentials = new NetworkCredential(_emailConfiguration.SmtpUsername, _emailConfiguration.SmtpPassword);
                smtpClient.EnableSsl = _emailConfiguration.EnableSsl;

                using (MailMessage mailMessage = new MailMessage())
                {
                    mailMessage.From = new MailAddress(_emailConfiguration.SmtpUsername);
                    mailMessage.Subject = subject;
                    mailMessage.Body = htmlMessage;
                    mailMessage.IsBodyHtml = true; // Ustaw to na false, jeśli treść wiadomości ma być zwykłym tekstem
                    mailMessage.To.Add(email);
                   await smtpClient.SendMailAsync(mailMessage);
                }
            }
        }
    }
}
