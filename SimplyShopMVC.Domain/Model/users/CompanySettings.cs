using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Domain.Model.users
{
    public class CompanySettings
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string NIP { get; set; }
        public string? Regon { get; set;}
        public string Street { get; set; }
        public string City { get; set; }
        public string PostCode { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string BankName { get; set; }
        public string BankAccount { get; set; }
        public string? AdditionalInfo { get; set; }
    }
}
