using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Application.ViewModels.Suppliers
{
    public class AddIncomGroupsVm
    {
        public IFormFile formFile { get; set; }
        public List<string> raportAddItem { get; set; }
        public bool removeItems { get; set; }
    }
}
