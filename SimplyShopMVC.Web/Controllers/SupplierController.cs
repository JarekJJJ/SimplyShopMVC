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
        //newIncom
        [HttpGet]
        public IActionResult AddNewIncomXml()
        {
            var newIncomresoult = _supplierService.LoadNewIncomGroupForView();
            return View(newIncomresoult);

        }
        [HttpPost]
        public async Task<IActionResult> AddNewIncomXml(AddIncomItemsVm incomItems, List<string> selectedCategoryId, IFormFile formFileXml)
        {
            AddIncomItemsVm returnRaport = new AddIncomItemsVm();
            incomItems.selectedCategory= selectedCategoryId;
            if (incomItems.formFile != null && incomItems.formFile.Length > 0)
            {

                var listItemsXML = XDocument.Load(incomItems.formFile.OpenReadStream());
                returnRaport = await _supplierService.LoadNewIncomItemsXML(incomItems, listItemsXML);
                return View(returnRaport);
            }
            if (!string.IsNullOrEmpty(incomItems.urlXml))
            {
                using (HttpClient client = new HttpClient())
                {
                    string xmlString = await client.GetStringAsync(incomItems.urlXml);
                    var listItemXmlfromUrl = XDocument.Parse(xmlString);
                    returnRaport = await _supplierService.LoadNewIncomItemsXML(incomItems, listItemXmlfromUrl);
                }
                return View(returnRaport);
            }
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
                        returnRaport = _supplierService.LoadOrinkItemsXML(incomItems, listItemsXML);
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
                    returnRaport = await _supplierService.UpdateIncomItemsXMLAsync(incomItems, listItemXmlfromUrl);
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
        public IActionResult AddGroupItems(int options)
        {
            //ConnectItemsToSupplierVm _connectItems = new ConnectItemsToSupplierVm();          
            var connectItems = _supplierService.LoadConnectItemsToSupplierVm(options);
            return View(connectItems);
        }
        [HttpPost]
        public async Task<IActionResult> AddGroupItems(ConnectItemsToSupplierVm connectItems, int options)
        {
            var returnRaport = await _supplierService.AddConnectItemsToSupplierVm(connectItems, options);
            var newconnectItems = _supplierService.LoadConnectItemsToSupplierVm(options);
            newconnectItems.raport = returnRaport.raport;

            return View(newconnectItems);
        }
    }
}
