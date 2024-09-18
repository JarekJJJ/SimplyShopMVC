using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Application.Interfaces
{
    public interface IEmailService
    {
        void SendEmail(string to, string subject, string body, List<string>? attachments);
        void SendEmail(string to, string subject, string body, byte[] attachments);
        void SendEmail(string to, string subject, string body);
        void SendAdminEmail(string subject, string body);
        void SendContactEmail(string from, string subject, string body, string ticket);
    }
}
