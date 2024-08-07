using Microsoft.Extensions.Configuration;
using SimplyShopMVC.Application.Interfaces;
using SimplyShopMVC.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SimplyShopMVC.Infrastructure;

namespace SimplyShopMVC.Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly IEmailConfiguration _emailConfiguration;

        public EmailService(IEmailConfiguration emailConfiguration)
        {
            _emailConfiguration = emailConfiguration;

        }
        public void SendEmail(string to, string subject, string body, List<string>? attachments)
        {
            using (SmtpClient smtpClient = new SmtpClient(_emailConfiguration.SmtpServer, _emailConfiguration.SmtpPort))
            {
                smtpClient.Credentials = new NetworkCredential(_emailConfiguration.SmtpUsername, _emailConfiguration.SmtpPassword);
                smtpClient.EnableSsl = _emailConfiguration.EnableSsl;

                using (MailMessage mailMessage = new MailMessage())
                {
                    mailMessage.From = new MailAddress(_emailConfiguration.SmtpUsername);
                    mailMessage.To.Add(to);
                    mailMessage.Subject = subject;
                    mailMessage.Body = body;
                    mailMessage.IsBodyHtml = true; // Ustaw to na false, jeśli treść wiadomości ma być zwykłym tekstem

                    // Dodaj załączniki
                    foreach (string attachmentPath in attachments)
                    {
                        Attachment attachment = new Attachment(attachmentPath, MediaTypeNames.Application.Octet);
                        ContentDisposition disposition = attachment.ContentDisposition;
                        disposition.CreationDate = File.GetCreationTime(attachmentPath);
                        disposition.ModificationDate = File.GetLastWriteTime(attachmentPath);
                        disposition.ReadDate = File.GetLastAccessTime(attachmentPath);

                        mailMessage.Attachments.Add(attachment);
                    }

                    smtpClient.Send(mailMessage);
                }
            }
        }
        public void SendEmail(string to, string subject, string body, byte[] pdfDocument)
        {
            using (SmtpClient smtpClient = new SmtpClient(_emailConfiguration.SmtpServer, _emailConfiguration.SmtpPort))
            {
                smtpClient.Credentials = new NetworkCredential(_emailConfiguration.SmtpUsername, _emailConfiguration.SmtpPassword);
                smtpClient.EnableSsl = _emailConfiguration.EnableSsl;

                using (MailMessage mailMessage = new MailMessage())
                {
                    mailMessage.From = new MailAddress(_emailConfiguration.SmtpUsername);
                    mailMessage.To.Add(to);
                    mailMessage.Subject = subject;
                    mailMessage.Body = body;
                    mailMessage.IsBodyHtml = true; // Ustaw to na false, jeśli treść wiadomości ma być zwykłym tekstem

                    // Dodaj załącznik
                    MemoryStream stream = new MemoryStream(pdfDocument);
                    Attachment attachment = new Attachment(stream, "GeneratedFile.pdf", MediaTypeNames.Application.Pdf);
                    mailMessage.Attachments.Add(attachment);
                    smtpClient.Send(mailMessage);
                }
            }
        }
        public void SendEmail(string to, string subject, string body)
        {
            using (SmtpClient smtpClient = new SmtpClient(_emailConfiguration.SmtpServer, _emailConfiguration.SmtpPort))
            {
                smtpClient.Credentials = new NetworkCredential(_emailConfiguration.SmtpUsername, _emailConfiguration.SmtpPassword);
                smtpClient.EnableSsl = _emailConfiguration.EnableSsl;

                using (MailMessage mailMessage = new MailMessage())
                {
                    mailMessage.From = new MailAddress(_emailConfiguration.SmtpUsername);
                    mailMessage.To.Add(to);
                    mailMessage.Subject = subject;
                    mailMessage.Body = body;
                    mailMessage.IsBodyHtml = true; // Ustaw to na false, jeśli treść wiadomości ma być zwykłym tekstem               
                    smtpClient.Send(mailMessage);
                }
            }
        }
        public void SendAdminEmail(string subject, string body)
        {
            using (SmtpClient smtpClient = new SmtpClient(_emailConfiguration.SmtpServer, _emailConfiguration.SmtpPort))
            {
                var to = _emailConfiguration.AdminEmail;
                smtpClient.Credentials = new NetworkCredential(_emailConfiguration.SmtpUsername, _emailConfiguration.SmtpPassword);
                smtpClient.EnableSsl = _emailConfiguration.EnableSsl;

                using (MailMessage mailMessage = new MailMessage())
                {
                    mailMessage.From = new MailAddress(_emailConfiguration.SmtpUsername);
                    mailMessage.To.Add(to);
                    mailMessage.Subject = subject;
                    mailMessage.Body = body;
                    mailMessage.IsBodyHtml = true; // Ustaw to na false, jeśli treść wiadomości ma być zwykłym tekstem               
                    smtpClient.Send(mailMessage);
                }
            }
        }
    }
}
