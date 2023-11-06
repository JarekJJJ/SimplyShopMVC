using Microsoft.AspNetCore.Mvc;
using SimplyShopMVC.Application.Interfaces;
using SimplyShopMVC.Application.ViewModels.Suppliers;
using System.Xml.Linq;

namespace SimplyShopMVC.Web.Controllers
{
    public class SupplierController : Controller
    {
        private readonly ISupplierService _supplierService;
        public SupplierController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult AddIncomItemsXML()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddIncomItemsXML(IFormFile xmlFile)
        {
            if (xmlFile != null && xmlFile.Length > 0)
            {

                var listItemsXML = XDocument.Load(xmlFile.OpenReadStream());
                var raport = _supplierService.LoadIncomItemsXML(listItemsXML);
            }
            return View();
        }
    }
}
