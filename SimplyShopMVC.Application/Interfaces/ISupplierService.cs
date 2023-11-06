using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SimplyShopMVC.Application.Interfaces
{
    public interface ISupplierService
    {
        string LoadIncomItemsXML(XDocument xmlDocument);
    }
}
