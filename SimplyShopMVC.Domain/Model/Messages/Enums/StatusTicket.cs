using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SimplyShopMVC.Domain.Model.Messages.Enums;

public enum StatusTicket
{
    [Display(Name = "Nowy temat")]
    NewTicket,
    [Display(Name ="W trakcie")]
    InProgress,
    [Display(Name = "Zawieszony")]
    Suspended,
    [Display(Name = "Zakończony")]
    Finished



}
