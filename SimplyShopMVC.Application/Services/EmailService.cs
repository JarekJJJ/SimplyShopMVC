using Microsoft.Extensions.Configuration;
using SimplyShopMVC.Application.Interfaces;
using SimplyShopMVC.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailConfiguration _emailConfiguration;

        public EmailService(IConfiguration configuration)
        {
            _emailConfiguration = configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
        }
        public void SendEmail(string to, string subject, string body, List<string>? attachments)
        {
            throw new NotImplementedException();
        }
    }
}
