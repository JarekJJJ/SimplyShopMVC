using Microsoft.Extensions.Configuration;
using SimplyShopMVC.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Infrastructure
{
    public class EmailConfiguration : IEmailConfiguration 
    {
        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpUsername { get; set; }
        public string SmtpPassword { get; set; }
        public bool EnableSsl { get; set; }
    }
}
