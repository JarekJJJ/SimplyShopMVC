using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Domain.Model.Messages.Enums;

public enum StatusMessage
{
    [Display(Name ="Wysłano wiadomość")]
    Delivered,
    [Display(Name ="Odczytano")]
    Read,
    [Display(Name = "Wysłano odpowiedź")]
    Replied,
    [Display(Name = "Zakończono")]
    Finished

}
