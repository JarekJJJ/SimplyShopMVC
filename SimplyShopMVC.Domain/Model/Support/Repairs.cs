using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Domain.Model.Support
{
    public class Repairs
    {
        public int Id { get; set; }
        public string ReportName { get; set; }
        public string ReportDescription { get; set; } = string.Empty;
        public string ReportType { get; set; }// dodać osobnę klasę z rodzajami raportów
        public string RepairStatus { get; set; } // Dodać klasę z listą statusów
        public string HardwareName { get; set; }
        public string HardwareDescription { get; set;} = string.Empty;
        public string SerialNumber { get; set; }
        public string Accessories { get; set;} = string.Empty;
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }


    }
}
