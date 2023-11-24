using Microsoft.AspNetCore.Mvc;
using SimplyShopMVC.Application.Interfaces;
using SimplyShopMVC.Application.ViewModels.Item;
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
        [HttpGet]
        public IActionResult AddIncomGroupsXML()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddIncomGroupsXML(AddIncomGroupsVm incomGroups)
        {
            if(incomGroups.formFile != null && incomGroups.formFile.Length > 0)
            {
                var listGroupXml = XDocument.Load(incomGroups.formFile.OpenReadStream());
                var returnRaport = _supplierService.AddIncomGroupsXML(incomGroups, listGroupXml);
                return View(returnRaport);
            }
            return View();
        }
        [HttpGet]
        public IActionResult AddGroupItems()
        {
            //ConnectItemsToSupplierVm _connectItems = new ConnectItemsToSupplierVm();
            var connectItems = _supplierService.LoadConnectItemsToSupplierVm();
            return View(connectItems);
        }
        [HttpPost]
        public IActionResult AddGroupItems(ConnectItemsToSupplierVm connectItems)
        {
           var returnRaport =  _supplierService.AddConnectItemsToSupplierVm(connectItems);
            var newconnectItems = _supplierService.LoadConnectItemsToSupplierVm();
            newconnectItems.raport = returnRaport.raport;

            return View(newconnectItems);
        }
    }
}
