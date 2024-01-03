using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Domain.Model.users
{
    public class UserDetail
    {
        public int Id { get; set; }
        public string userId { get; set; }
        public bool isClientBusiness { get; set; }
        public string ContactPerson { get; set; }
        public string? NIP { get; set; }
        public string? EmailAddress { get; set; }
        public string? PhoneNumber { get; set; }
        public string? City { get; set; }
        public string? Region { get; set; }
        public string? PostalCode { get; set; }
        public string? Country { get; set; }
        public string? Street { get; set; }
    }
}