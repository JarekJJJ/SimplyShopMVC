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
            AddIncomItemsVm _incomItem = new AddIncomItemsVm();
            XDocument _doc= new XDocument();
            var incomItem = _supplierService.LoadIncomItemsXML(_incomItem, _doc);
            return View(incomItem);
        }
        [HttpPost]
        public IActionResult AddIncomItemsXML(AddIncomItemsVm incomItems)
        {
            if (incomItems.formFile != null && incomItems.formFile.Length > 0)
            {

                var listItemsXML = XDocument.Load( incomItems.formFile.OpenReadStream());
                var returnRaport = _supplierService.LoadIncomItemsXML(incomItems, listItemsXML);
                return View(returnRaport);
            }
           return View();
        }
    }
}
