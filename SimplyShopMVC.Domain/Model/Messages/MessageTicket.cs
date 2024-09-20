using SimplyShopMVC.Domain.Model.Messages.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Domain.Model.Messages
{
    public class MessageTicket
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public StatusTicket StatusTicket { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime Updated { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; }

        public virtual ICollection<Message> Messages { get;set; }
    }
}
