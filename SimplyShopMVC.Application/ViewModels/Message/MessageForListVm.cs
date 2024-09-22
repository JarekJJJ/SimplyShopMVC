using AutoMapper;
using SimplyShopMVC.Application.Mapping;
using SimplyShopMVC.Domain.Model.Messages.Enums;
using SimplyShopMVC.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Application.ViewModels.Message
{
    public class MessageForListVm: IMapFrom<SimplyShopMVC.Domain.Model.Messages.Message>
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Pole jest wymagane.")]
        [EmailAddress(ErrorMessage ="Nieprawidłowy adres email")]
        public string SenderAddress { get; set; } = string.Empty;
        public string RecipientMessage { get; set; } = string.Empty;
        public string ReplayTo { get; set; } = string.Empty;
        [Required(ErrorMessage = "Pole jest wymagane.")]
        public string Title { get; set; } = string.Empty;
        [Required(ErrorMessage = "Pole jest wymagane.")]
        public string Body { get; set; } = string.Empty;
        public string? SenderUserId { get; set; }
        public string TicketMessage { get; set; } = string.Empty;
        public bool IsDeleted { get; set; } = false;
        public int MessageTicketId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime UpdateDateTime { get; set; } = DateTime.Now;
        public StatusMessage StatusMessage { get; set; }
        public string? SenderIpAddress { get; set; } = string.Empty;
        [MustBeTrue(ErrorMessage= "Musisz zaakceptować warunki.")]
        [Display(Name = "Akceptuję Politykę Prywatności")]
        public bool PrivacyPolicy { get; set; } 
        public string? senderTrueEmail { get; set; }
        public string? AntySpamResult { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<MessageForListVm, SimplyShopMVC.Domain.Model.Messages.Message>().ReverseMap();
        }
    }
}
