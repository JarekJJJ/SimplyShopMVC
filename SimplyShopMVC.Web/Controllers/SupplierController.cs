using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimplyShopMVC.Application.Interfaces;
using SimplyShopMVC.Application.ViewModels.Item;
using SimplyShopMVC.Application.ViewModels.Suppliers;
using System.Data;
using System.Security.Policy;
using System.Xml.Linq;

namespace SimplyShopMVC.Web.Controllers
{
    [Authorize(Roles = "Admin")]
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
        //Incom
        [HttpGet]
        public IActionResult AddIncomItemsXML()
        {
            AddIncomItemsVm _incomItem = new AddIncomItemsVm();
            XDocument _doc = new XDocument();
            var incomItem = _supplierService.LoadIncomItemsXML(_incomItem, _doc);
            return View(incomItem);
        }
        [HttpPost]
        public async Task<IActionResult> AddIncomItemsXML(AddIncomItemsVm incomItems)
        {
            AddIncomItemsVm returnRaport = new AddIncomItemsVm();
            switch (incomItems.warehouseId)
            {
                case 3: //Incom
                    if (incomItems.formFile != null && incomItems.formFile.Length > 0)
                    {

                        var listItemsXML = XDocument.Load(incomItems.formFile.OpenReadStream());
                        returnRaport = _supplierService.LoadIncomItemsXML(incomItems, listItemsXML);
                        return View(returnRaport);
                    }
                    if (!string.IsNullOrEmpty(incomItems.urlXml))
                    {
                        using (HttpClient client = new HttpClient())
                        {
                            string xmlString = await client.GetStringAsync(incomItems.urlXml);
                            var listItemXmlfromUrl = XDocument.Parse(xmlString);
                            returnRaport = _supplierService.LoadIncomItemsXML(incomItems, listItemXmlfromUrl);
                        }
                        return View(returnRaport);
                    }
                    break;
                case 4: //Orink
                    if (incomItems.formFile != null && incomItems.formFile.Length > 0)
                    {

                        var listItemsXML = XDocument.Load(incomItems.formFile.OpenReadStream());
                        //returnRaport = _supplierService.LoadIncomItemsXML(incomItems, listItemsXML);
                        return View(returnRaport);
                    }
                    break;
                default:
                    break;
            }
          
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> UpdateIncomItemsXML(AddIncomItemsVm incomItems)
        {
            AddIncomItemsVm returnRaport = new AddIncomItemsVm();
            if (incomItems.formFile != null && incomItems.formFile.Length > 0)
            {

                var listItemsXML = XDocument.Load(incomItems.formFile.OpenReadStream());
                returnRaport = _supplierService.UpdateIncomItemsXML(incomItems, listItemsXML);
                return View(returnRaport);
            }
            if (!string.IsNullOrEmpty(incomItems.urlXml))
            {
                using (HttpClient client = new HttpClient())
                {
                    string xmlString = await client.GetStringAsync(incomItems.urlXml);
                    var listItemXmlfromUrl = XDocument.Parse(xmlString);
                    returnRaport = _supplierService.UpdateIncomItemsXML(incomItems, listItemXmlfromUrl);
                }
                AddIncomItemsVm _incomItem = new AddIncomItemsVm();
                XDocument _doc = new XDocument();
                var incomItem = _supplierService.LoadIncomItemsXML(_incomItem, _doc);
                returnRaport.warehouseForListVm = incomItem.warehouseForListVm;
                return View("~/Views/Supplier/AddIncomItemsXML.cshtml", returnRaport);
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
            if (incomGroups.formFile != null && incomGroups.formFile.Length > 0)
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
            var returnRaport = _supplierService.AddConnectItemsToSupplierVm(connectItems);
            var newconnectItems = _supplierService.LoadConnectItemsToSupplierVm();
            newconnectItems.raport = returnRaport.raport;

            return View(newconnectItems);
        }
    }
}
