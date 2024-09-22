using AutoMapper;
using SimplyShopMVC.Application.Mapping;
using SimplyShopMVC.Domain.Model.Messages.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Application.ViewModels.Message
{
    public  class MessageTicketForListVm: IMapFrom<SimplyShopMVC.Domain.Model.Messages.MessageTicket>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public StatusTicket StatusTicket { get; set; }
        public DateTime Created { get; set; } 
        public DateTime Updated { get; set; } 
        public bool IsDeleted { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<MessageTicketForListVm, SimplyShopMVC.Domain.Model.Messages.MessageTicket>().ReverseMap();
        }
    }
}

