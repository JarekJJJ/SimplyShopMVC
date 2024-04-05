using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Hosting;
using SimplyShopMVC.Application.Interfaces;
using SimplyShopMVC.Application.ViewModels.Item;
using SimplyShopMVC.Application.ViewModels.Suppliers;
using SimplyShopMVC.Domain.Interface;
using SimplyShopMVC.Domain.Model.Suppliers;
using SimplyShopMVC.Application.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;
using SimplyShopMVC.Domain.Model;
using System.Net.Mail;
using iText.StyledXmlParser.Jsoup.Nodes;
using SimplyShopMVC.Infrastructure.Migrations;

namespace SimplyShopMVC.Application.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly ISupplierRepository _supplierRepo;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHost;
        private readonly IItemRepository _itemRepo;
        private readonly IGroupItemRepository _groupItemRepo;
        private readonly IOmnibusPriceRepository _omnibusPriceRepo;
        private readonly ICategoryTagsRepository _categoryTagsRepo;
        private readonly IOrinkRepository _orinkRepo;
        private readonly IHttpClientFactory _httpClientFactory;

        public SupplierService(IItemRepository itemRepo, IMapper mapper, IWebHostEnvironment webHost, ISupplierRepository supplierRepo,
            IGroupItemRepository groupItemRepo, IOmnibusPriceRepository omnibusPriceRepo, ICategoryTagsRepository categoryTagsRepo, IOrinkRepository orinkRepo, IHttpClientFactory httpClientFactory)
        {
            _itemRepo = itemRepo;
            _mapper = mapper;
            _webHost = webHost;
            _supplierRepo = supplierRepo;
            _groupItemRepo = groupItemRepo;
            _omnibusPriceRepo = omnibusPriceRepo;
            _categoryTagsRepo = categoryTagsRepo;
            _orinkRepo = orinkRepo;
            _httpClientFactory = httpClientFactory;
        }
        //newIncom
        public AddIncomItemsVm LoadNewIncomGroupForView()
        {
            AddIncomItemsVm resoultListGroup= new AddIncomItemsVm();
            var mapedResoultVm = _supplierRepo.GetAllIncomGroup().ProjectTo<IncomGroupForListVm>(_mapper.ConfigurationProvider).ToList();
            var mappedWare = _itemRepo.GetAllWarehouses().ProjectTo<WarehouseForListVm>(_mapper.ConfigurationProvider).ToList();
            resoultListGroup.listIncomGroup = mapedResoultVm;
            resoultListGroup.warehouseForListVm= mappedWare;

            return resoultListGroup;
        }
        public async Task<AddIncomItemsVm> LoadNewIncomItemsXML(AddIncomItemsVm incomItems, XDocument xmlDocument)
        {
            int countItemUpdate = 0;
            int countItemAdd = 0;
            int countItemRemove = 0;
            int countImageAdd = 0;
            int _categoryNr;
            AddIncomItemsVm returnRaport = new AddIncomItemsVm();
            List<string> list = new List<string>();
            returnRaport.raportAddItem = new List<string>();
            List<int> idSelectedCategory= new List<int>();
            foreach (var categoryNr in incomItems.selectedCategory)
            {
                List<IncomGroup> listCategoryToAddIncomItems = new List<IncomGroup>();
                int.TryParse(categoryNr, out _categoryNr);
                listCategoryToAddIncomItems = _supplierRepo.GetAllIncomGroup().Where(a => a.GroupId == _categoryNr || a.GroupIdHome == _categoryNr).ToList();
                if (listCategoryToAddIncomItems != null)
                {
                    foreach(var listCategory in listCategoryToAddIncomItems)
                    {
                        idSelectedCategory.Add(listCategory.GroupId);
                    }
                }
               
            }
            
            if (incomItems.warehouseId != 0)
            {
                if (incomItems.removeItems == true)
                {
                    var itemFromSelectedWarehouse = _supplierRepo.GetAllIncom().Where(i => i.warehouseId == incomItems.warehouseId).ToList();
                    foreach (var item in itemFromSelectedWarehouse)
                    {
                       await _supplierRepo.DeleteIncomItemAsync(item.Id);
                        countItemRemove++;
                    }
                }
                CultureInfo cultureInfo = new CultureInfo("pl-PL");
                cultureInfo.NumberFormat.NumberDecimalSeparator = ",";
                DateTime dateTime = DateTime.Now;

                foreach (XElement elementXml in xmlDocument.Root.Elements("produkt"))
                {
                    bool continueTemp = false;
                    try
                    {
                        XElement grupa_towarowa = elementXml.Element("grupa_towarowa");
                        if (grupa_towarowa.Value != null)
                        {
                            int tempXmlGroupId;
                          bool okParse = int.TryParse(grupa_towarowa.Value, out tempXmlGroupId);
                            if (okParse)
                            {
                                foreach (var _selectedGroup in idSelectedCategory)
                                {
                                    if (_selectedGroup == tempXmlGroupId)
                                    {
                                        continueTemp = true;
                                    }
                                }
                            }
                        }
                        if (continueTemp)
                        {
                            XElement nazwa_grupy_towarowej = elementXml.Element("nazwa_grupy_towarowej");
                            XElement symbol_produktu = elementXml.Element("symbol_produktu");
                            XElement nazwa_produktu = elementXml.Element("nazwa_produktu");
                            XElement symbol_producenta = elementXml.Element("symbol_producenta");
                            XElement ean = elementXml.Element("ean");
                            XElement nazwa_producenta = elementXml.Element("nazwa_producenta");
                            XElement opisProduktu = elementXml.Element("opis");
                            string firstLineDescription = "<table class=\"table table-striped-columns\">";
                            string lastLineDescription = "</table>";
                            string lineDescription = string.Empty;
                            string fullDescription = ("");
                            if (opisProduktu != null)
                            {
                                foreach (XElement parametrOpisu in opisProduktu.Elements("parametr"))
                                {
                                    string paramName = parametrOpisu.Element("nazwa")?.Value;
                                    string paramValue = parametrOpisu.Element("wartosc")?.Value;
                                    string newLineDescription = $"<tr><td>{paramName}</td><td>{paramValue}</td></tr>";
                                    lineDescription = lineDescription + newLineDescription;
                                }
                                fullDescription = firstLineDescription + lineDescription + lastLineDescription;
                            }
                            else
                            {
                                fullDescription = ("brak opisu");
                            }
                            List<string> linkImage = new List<string>();
                            string link = elementXml.Element("link_do_zdjecia_produktu")?.Value;
                            linkImage.Add(link);
                            foreach (XElement addLink in elementXml.Elements("media"))
                            {
                                string newLink = addLink.Element("url")?.Value;
                                linkImage.Add(newLink);
                            }
                            XElement stan_magazynowy = elementXml.Element("stan_magazynowy");
                            XElement cena = elementXml.Element("cena");
                            XElement opakowanie = elementXml.Element("opakowanie");
                            XElement dlugosc = opakowanie.Element("dlugosc");
                            XElement szerokosc = opakowanie.Element("szerokosc");
                            XElement wysokosc = opakowanie.Element("wyskosc");
                            XElement waga = opakowanie.Element("waga");
                            IncomItemsForListVm itemVm = new IncomItemsForListVm();
                            itemVm.warehouseId = incomItems.warehouseId;
                            itemVm.grupa_towarowa = grupa_towarowa.Value;
                            itemVm.nazwa_grupy_towarowej = nazwa_grupy_towarowej.Value;
                            itemVm.symbol_produktu = symbol_produktu.Value;
                            itemVm.nazwa_produktu = nazwa_produktu.Value;
                            itemVm.symbol_producenta = symbol_producenta.Value;
                            itemVm.ean = xmlHelpers.eanController(ean.Value);
                            itemVm.nazwa_producenta = nazwa_producenta.Value;
                            itemVm.opis = fullDescription;
                            itemVm.urlImage = linkImage.FirstOrDefault();
                            if (String.IsNullOrEmpty(itemVm.urlImage))
                            {
                                itemVm.urlImage = " ";
                            }
                            //string cleanedText = new string(stan_magazynowy.Value.Where(char.IsDigit).ToArray());                             
                            itemVm.stan_magazynowy = changeToInt(stan_magazynowy.Value);
                            itemVm.cena = decimal.Parse(cena.Value, cultureInfo);
                            itemVm.dlugosc = changeToDecimal(dlugosc.Value);
                            itemVm.szerokosc = changeToDecimal(szerokosc.Value);
                            itemVm.wysokosc = changeToDecimal(wysokosc.Value);
                            itemVm.waga = changeToDecimal(waga.Value);
                            itemVm.createDate = dateTime; // Tutaj zrobić podział na update i Add !!!
                            var itemToCheck = _supplierRepo.GetAllIncom().FirstOrDefault(i => i.symbol_produktu == itemVm.symbol_produktu);
                            var mapedItemVm = _mapper.Map<Incom>(itemVm);
                            int itemId = 0;
                            if (itemToCheck != null && !string.IsNullOrEmpty(itemVm.ean))
                            {
                                mapedItemVm.createDate = itemToCheck.createDate;
                                mapedItemVm.updateTime = dateTime;
                              await _supplierRepo.UpdateIncomAsync(mapedItemVm);
                               await UpdateItemInShopAsync(mapedItemVm);
                                countItemUpdate++;
                                itemId = itemToCheck.Id;
                            }
                            if (itemToCheck == null && !string.IsNullOrEmpty(itemVm.ean))
                            {
                                itemId = await _supplierRepo.AddIncomItemAsync(mapedItemVm);
                                //var result = ImageHelper.SaveImageFromUrl(linkImage, itemVm.ean, _webHost);
                                //countImageAdd = countImageAdd + result;
                                countItemAdd++;
                            }
                            OmnibusPriceToListVm omnibusPrice = new OmnibusPriceToListVm();
                            var mapedOmnibusPrice = _mapper.Map<OmnibusPrice>(omnibusPrice);
                            mapedOmnibusPrice.PriceN = itemVm.cena;
                            mapedOmnibusPrice.Ean = itemVm.ean;
                            mapedOmnibusPrice.ChangeTime = dateTime;
                            mapedOmnibusPrice.WarehouseId = itemVm.warehouseId;
                           await _omnibusPriceRepo.AddOmnibusPriceAsync(mapedOmnibusPrice);
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
            if (countItemAdd > 0 || countItemUpdate > 0)
            {
                returnRaport.raportAddItem.Add($"Usunięto rekordów: {countItemRemove}");
                returnRaport.raportAddItem.Add($"Dodano rekordów: {countItemAdd}");
                returnRaport.raportAddItem.Add($"Zmodyfikowano rekordów: {countItemUpdate}");
                returnRaport.raportAddItem.Add($"Dodano zdjęć: {countImageAdd}");
                returnRaport.raportAddItem.Add("ok");
            }
            returnRaport.warehouseForListVm = _itemRepo.GetAllWarehouses().ProjectTo<WarehouseForListVm>(_mapper.ConfigurationProvider).ToList();
            var mapedResoultVm = _supplierRepo.GetAllIncomGroup().ProjectTo<IncomGroupForListVm>(_mapper.ConfigurationProvider).ToList();
            returnRaport.listIncomGroup = mapedResoultVm;
            return returnRaport;
        }

        //Orink
        public AddIncomItemsVm LoadOrinkItemsXML(AddIncomItemsVm orinkItems, XDocument xmlDocument)
        {
            int countItemUpdate = 0;
            int countItemAdd = 0;
            int countItemRemove = 0;
            AddIncomItemsVm returnRaport = new AddIncomItemsVm();
            List<string> list = new List<string>();
            returnRaport.raportAddItem = new List<string>();
            if (orinkItems.removeItems == true)
            {
                var items = _orinkRepo.GetAllOrink();
                countItemRemove = items.Count();
                _orinkRepo.DeleteAllOrinkItem(items);
            }
            if (orinkItems.warehouseId != 0)
            {
                CultureInfo cultureInfo = new CultureInfo("pl-PL");
                cultureInfo.NumberFormat.NumberDecimalSeparator = ".";
                DateTime dateTime = DateTime.Now;
                foreach (XElement elementXml in xmlDocument.Root.Elements("product"))
                {
                    //try
                    //{
                    XElement imgLink = elementXml.Element("img");
                    XElement symbol_producenta = elementXml.Element("code");
                    XElement cena = elementXml.Element("price");
                    XElement ilosc = elementXml.Element("available");
                    XElement name = elementXml.Element("description");
                    XElement eanCode = elementXml.Element("ean_code");
                    XElement color = elementXml.Element("color");
                    OrinkItemForListVm orinkItem = new OrinkItemForListVm();
                    orinkItem.name = !String.IsNullOrEmpty(name.Value) ? name.Value : string.Empty;
                    orinkItem.description = !String.IsNullOrEmpty(name.Value) ? name.Value : string.Empty;
                    orinkItem.color = !String.IsNullOrEmpty(color.Value) ? color.Value : string.Empty;
                    var tusze = _orinkRepo.GetAllorinkGroup().FirstOrDefault(g => g.Name == "Tusze");
                    var tonery = _orinkRepo.GetAllorinkGroup().FirstOrDefault(t => t.Name == "Tonery");
                    if (orinkItem.name.Contains("Tusz") && tusze != null)
                    {
                        orinkItem.OrinkGroupId = tusze.Id;
                    }
                    if (orinkItem.name.Contains("Toner") && tonery != null)
                    {
                        orinkItem.OrinkGroupId = tonery.Id;
                    }
                    if (orinkItem.OrinkGroupId == 0)
                    {
                        orinkItem.OrinkGroupId = 3;
                    }
                    List<string> listProducents = new List<string>
                        { "HP", "EPSON", "BROTHER", "XEROX", "LEXMARK", "CANON", "OKI", "SAMSUNG",
                        "RICOH", "PANASONIC", "KONICA MINOLTA", "KYOCERA", "DELL"};
                    StringBuilder producenci = new StringBuilder();
                    foreach (var item in listProducents)
                    {
                        if (orinkItem.name.IndexOf(item, StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            producenci.Append(item);
                            producenci.Append(" ");
                        }
                    }
                    orinkItem.printerProducent = !String.IsNullOrEmpty(producenci.ToString()) ? producenci.ToString() : string.Empty;
                    orinkItem.symbol_producenta = !String.IsNullOrEmpty(symbol_producenta.Value) ? symbol_producenta.Value : string.Empty;
                    orinkItem.ean = !String.IsNullOrEmpty(eanCode.Value) ? eanCode.Value : string.Empty;
                    orinkItem.stan_magazynowy = int.Parse(ilosc.Value);
                    orinkItem.cenaNetto = decimal.Parse(cena.Value, cultureInfo);
                    orinkItem.imgLink = !String.IsNullOrEmpty(imgLink.Value) ? imgLink.Value : string.Empty;
                    //orinkItem.createDate = DateTime.Now;
                    var itemToCheck = _orinkRepo.GetAllOrink().FirstOrDefault(i => i.ean == orinkItem.ean);
                    int itemId = 0;
                    if (itemToCheck != null)
                    {
                        orinkItem.Id = itemToCheck.Id;
                        orinkItem.updateTime = DateTime.Now;
                        orinkItem.warehouseId = itemToCheck.warehouseId;
                        var mappedItem = _mapper.Map<Orink>(orinkItem);
                        _orinkRepo.UpdateOrink(mappedItem);
                        var orinkItemWare = _itemRepo.GetAllItems().FirstOrDefault(i => i.EanCode == mappedItem.ean);
                        if ((orinkItemWare != null) && (orinkItem.stan_magazynowy > 0))
                        {
                            var oItemWare = _itemRepo.GetAllItemWarehouses().FirstOrDefault(i => i.ItemId == orinkItemWare.Id);
                            if (oItemWare != null)
                            {
                                oItemWare.NetPurchasePrice = orinkItem.cenaNetto;
                                oItemWare.Quantity = orinkItem.stan_magazynowy;
                                _itemRepo.UpdateItemWarehouse(oItemWare);

                                OmnibusPriceToListVm omnibusPrice = new OmnibusPriceToListVm();
                                var mapedOmnibusPrice = _mapper.Map<OmnibusPrice>(omnibusPrice);
                                mapedOmnibusPrice.PriceN = orinkItem.cenaNetto;
                                mapedOmnibusPrice.Ean = orinkItem.ean;
                                mapedOmnibusPrice.ChangeTime = DateTime.Now;
                                mapedOmnibusPrice.WarehouseId = orinkItems.warehouseId;
                                _omnibusPriceRepo.AddOmnibusPrice(mapedOmnibusPrice);
                                countItemUpdate++;
                            }
                        }
                    }
                    if (itemToCheck == null)
                    {
                        orinkItem.createDate = DateTime.Now;
                        orinkItem.warehouseId = orinkItems.warehouseId;
                        var mappedItem = _mapper.Map<Orink>(orinkItem);
                        _orinkRepo.AddOrinkItem(mappedItem);
                        countItemAdd++;
                    }
                    //}
                    //    catch (Exception ex)
                    //{
                    //    throw;
                    //}
                }
            }
            if ((countItemAdd > 0) || (countItemUpdate > 0))
            {
                returnRaport.raportAddItem.Add($"Usunięto rekordów: {countItemRemove}");
                returnRaport.raportAddItem.Add($"Dodano rekordów: {countItemAdd}");
                returnRaport.raportAddItem.Add($"Zmodyfikowano rekordów: {countItemUpdate}");
                // returnRaport.raportAddItem.Add($"Dodano zdjęć: {countImageAdd}");
                returnRaport.raportAddItem.Add("ok");
            }
            returnRaport.warehouseForListVm = _itemRepo.GetAllWarehouses().ProjectTo<WarehouseForListVm>(_mapper.ConfigurationProvider).ToList();

            return returnRaport;
        }
        //Incom
        public AddIncomItemsVm UpdateIncomItemsXML(AddIncomItemsVm incomItems, XDocument xmlDocument)
        {
            int countItemUpdate = 0;
            int countItemAdd = 0;
            int removeCountItem = 0;
            AddIncomItemsVm returnRaport = new AddIncomItemsVm();
            List<string> list = new List<string>();
            returnRaport.raportAddItem = new List<string>();
            if (incomItems.warehouseId != 0)
            {
                CultureInfo cultureInfo = new CultureInfo("pl-PL");
                cultureInfo.NumberFormat.NumberDecimalSeparator = ",";
                DateTime dateTime = DateTime.Now;
                foreach (XElement elementXml in xmlDocument.Root.Elements("produkt"))
                {
                    try
                    {
                        XElement grupa_towarowa = elementXml.Element("grupa_towarowa");
                        XElement nazwa_grupy_towarowej = elementXml.Element("nazwa_grupy_towarowej");
                        XElement symbol_produktu = elementXml.Element("symbol_produktu");
                        XElement nazwa_produktu = elementXml.Element("nazwa_produktu");
                        XElement symbol_producenta = elementXml.Element("symbol_producenta");
                        XElement ean = elementXml.Element("ean");
                        XElement nazwa_producenta = elementXml.Element("nazwa_producenta");
                        XElement stan_magazynowy = elementXml.Element("stan_magazynowy");
                        XElement cena = elementXml.Element("cena");
                        IncomItemsForListVm itemVm = new IncomItemsForListVm();
                        itemVm.warehouseId = incomItems.warehouseId;
                        itemVm.symbol_produktu = symbol_produktu.Value;
                        itemVm.nazwa_produktu = nazwa_produktu.Value;
                        itemVm.ean = xmlHelpers.eanController(ean.Value);
                        itemVm.stan_magazynowy = changeToInt(stan_magazynowy.Value);
                        itemVm.cena = decimal.Parse(cena.Value, cultureInfo);
                        itemVm.updateTime = dateTime; // Tutaj zrobić podział na update i Add !!!
                        var itemToCheck = _supplierRepo.GetAllIncom().FirstOrDefault(i => i.symbol_produktu == itemVm.symbol_produktu);
                        var mapedItemVm = _mapper.Map<IncomItemsForListVm>(itemToCheck);
                        int itemId = 0;
                        if (itemToCheck != null && !string.IsNullOrEmpty(itemVm.ean))
                        {
                            itemToCheck.cena = itemVm.cena;
                            itemToCheck.stan_magazynowy = itemVm.stan_magazynowy;
                            itemToCheck.updateTime = dateTime;
                            _supplierRepo.UpdateIncom(itemToCheck);
                            UpdateItemInShop(itemToCheck);
                            countItemUpdate++;
                            itemId = itemToCheck.Id;
                            OmnibusPriceToListVm omnibusPrice = new OmnibusPriceToListVm();
                            var mapedOmnibusPrice = _mapper.Map<OmnibusPrice>(omnibusPrice);
                            mapedOmnibusPrice.PriceN = itemVm.cena;
                            mapedOmnibusPrice.Ean = itemVm.ean;
                            mapedOmnibusPrice.ChangeTime = dateTime;
                            mapedOmnibusPrice.WarehouseId = itemVm.warehouseId;
                            _omnibusPriceRepo.AddOmnibusPrice(mapedOmnibusPrice);
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
            return returnRaport;
        }
        public async Task<AddIncomItemsVm> UpdateIncomItemsXMLAsync(AddIncomItemsVm incomItems, XDocument xmlDocument)
        {
            int countItemUpdate = 0;
            int countItemAdd = 0;
            int removeCountItem = 0;
            AddIncomItemsVm returnRaport = new AddIncomItemsVm();
            List<string> list = new List<string>();
            returnRaport.raportAddItem = new List<string>();
            if (incomItems.warehouseId != 0)
            {
                CultureInfo cultureInfo = new CultureInfo("pl-PL");
                cultureInfo.NumberFormat.NumberDecimalSeparator = ",";
                DateTime dateTime = DateTime.Now;
                foreach (XElement elementXml in xmlDocument.Root.Elements("produkt"))
                {
                    try
                    {
                        XElement grupa_towarowa = elementXml.Element("grupa_towarowa");
                        XElement nazwa_grupy_towarowej = elementXml.Element("nazwa_grupy_towarowej");
                        XElement symbol_produktu = elementXml.Element("symbol_produktu");
                        XElement nazwa_produktu = elementXml.Element("nazwa_produktu");
                        XElement symbol_producenta = elementXml.Element("symbol_producenta");
                        XElement ean = elementXml.Element("ean");
                        XElement nazwa_producenta = elementXml.Element("nazwa_producenta");
                        XElement stan_magazynowy = elementXml.Element("stan_magazynowy");
                        XElement cena = elementXml.Element("cena");
                        IncomItemsForListVm itemVm = new IncomItemsForListVm();
                        itemVm.warehouseId = incomItems.warehouseId;
                        itemVm.symbol_produktu = symbol_produktu.Value;
                        itemVm.nazwa_produktu = nazwa_produktu.Value;
                        itemVm.ean = xmlHelpers.eanController(ean.Value);
                        itemVm.stan_magazynowy = changeToInt(stan_magazynowy.Value);
                        itemVm.cena = decimal.Parse(cena.Value, cultureInfo);
                        itemVm.updateTime = dateTime; // Tutaj zrobić podział na update i Add !!!
                        var itemToCheck = _supplierRepo.GetAllIncom().FirstOrDefault(i => i.symbol_produktu == itemVm.symbol_produktu);
                        var mapedItemVm = _mapper.Map<IncomItemsForListVm>(itemToCheck);
                        int itemId = 0;
                        if (itemToCheck != null && !string.IsNullOrEmpty(itemVm.ean))
                        {
                            itemToCheck.cena = itemVm.cena;
                            itemToCheck.stan_magazynowy = itemVm.stan_magazynowy;
                            itemToCheck.updateTime = dateTime;
                            await _supplierRepo.UpdateIncomAsync(itemToCheck);
                            await UpdateItemInShopAsync(itemToCheck);
                            countItemUpdate++;
                            itemId = itemToCheck.Id;
                            OmnibusPriceToListVm omnibusPrice = new OmnibusPriceToListVm();
                            var mapedOmnibusPrice = _mapper.Map<OmnibusPrice>(omnibusPrice);
                            mapedOmnibusPrice.PriceN = itemVm.cena;
                            mapedOmnibusPrice.Ean = itemVm.ean;
                            mapedOmnibusPrice.ChangeTime = dateTime;
                            mapedOmnibusPrice.WarehouseId = itemVm.warehouseId;
                            await _omnibusPriceRepo.AddOmnibusPriceAsync(mapedOmnibusPrice);
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
            return returnRaport;
        }

        public AddIncomItemsVm LoadIncomItemsXML(AddIncomItemsVm incomItems, XDocument xmlDocument)
        {
            int countItemUpdate = 0;
            int countItemAdd = 0;
            int countItemRemove = 0;
            int countImageAdd = 0;
            AddIncomItemsVm returnRaport = new AddIncomItemsVm();
            List<string> list = new List<string>();
            returnRaport.raportAddItem = new List<string>();
            if (incomItems.warehouseId != 0)
            {
                if (incomItems.removeItems == true)
                {
                    var itemFromSelectedWarehouse = _supplierRepo.GetAllIncom().Where(i => i.warehouseId == incomItems.warehouseId).ToList();
                    foreach (var item in itemFromSelectedWarehouse)
                    {
                        _supplierRepo.DeleteIncomItem(item.Id);
                        countItemRemove++;
                    }
                }
                CultureInfo cultureInfo = new CultureInfo("pl-PL");
                cultureInfo.NumberFormat.NumberDecimalSeparator = ",";
                DateTime dateTime = DateTime.Now;
                foreach (XElement elementXml in xmlDocument.Root.Elements("produkt"))
                {
                    try
                    {
                        XElement grupa_towarowa = elementXml.Element("grupa_towarowa");
                        XElement nazwa_grupy_towarowej = elementXml.Element("nazwa_grupy_towarowej");
                        XElement symbol_produktu = elementXml.Element("symbol_produktu");
                        XElement nazwa_produktu = elementXml.Element("nazwa_produktu");
                        XElement symbol_producenta = elementXml.Element("symbol_producenta");
                        XElement ean = elementXml.Element("ean");
                        XElement nazwa_producenta = elementXml.Element("nazwa_producenta");
                        XElement opisProduktu = elementXml.Element("opis");
                        string firstLineDescription = "<table class=\"table table-striped-columns\">";
                        string lastLineDescription = "</table>";
                        string lineDescription = string.Empty;
                        string fullDescription = ("");
                        if (opisProduktu != null)
                        {
                            foreach (XElement parametrOpisu in opisProduktu.Elements("parametr"))
                            {
                                string paramName = parametrOpisu.Element("nazwa")?.Value;
                                string paramValue = parametrOpisu.Element("wartosc")?.Value;
                                string newLineDescription = $"<tr><td>{paramName}</td><td>{paramValue}</td></tr>";
                                lineDescription = lineDescription + newLineDescription;
                            }
                            fullDescription = firstLineDescription + lineDescription + lastLineDescription;
                        }
                        else
                        {
                            fullDescription = ("brak opisu");
                        }
                        List<string> linkImage = new List<string>();
                        string link = elementXml.Element("link_do_zdjecia_produktu")?.Value;
                        linkImage.Add(link);
                        foreach (XElement addLink in elementXml.Elements("media"))
                        {
                            string newLink = addLink.Element("url")?.Value;
                            linkImage.Add(newLink);
                        }
                        XElement stan_magazynowy = elementXml.Element("stan_magazynowy");
                        XElement cena = elementXml.Element("cena");
                        XElement opakowanie = elementXml.Element("opakowanie");
                        XElement dlugosc = opakowanie.Element("dlugosc");
                        XElement szerokosc = opakowanie.Element("szerokosc");
                        XElement wysokosc = opakowanie.Element("wyskosc");
                        XElement waga = opakowanie.Element("waga");
                        IncomItemsForListVm itemVm = new IncomItemsForListVm();
                        itemVm.warehouseId = incomItems.warehouseId;
                        itemVm.grupa_towarowa = grupa_towarowa.Value;
                        itemVm.nazwa_grupy_towarowej = nazwa_grupy_towarowej.Value;
                        itemVm.symbol_produktu = symbol_produktu.Value;
                        itemVm.nazwa_produktu = nazwa_produktu.Value;
                        itemVm.symbol_producenta = symbol_producenta.Value;
                        itemVm.ean = xmlHelpers.eanController(ean.Value);
                        itemVm.nazwa_producenta = nazwa_producenta.Value;
                        itemVm.opis = fullDescription;
                        //string cleanedText = new string(stan_magazynowy.Value.Where(char.IsDigit).ToArray());                             
                        itemVm.stan_magazynowy = changeToInt(stan_magazynowy.Value);
                        itemVm.cena = decimal.Parse(cena.Value, cultureInfo);
                        itemVm.dlugosc = changeToDecimal(dlugosc.Value);
                        itemVm.szerokosc = changeToDecimal(szerokosc.Value);
                        itemVm.wysokosc = changeToDecimal(wysokosc.Value);
                        itemVm.waga = changeToDecimal(waga.Value);
                        itemVm.createDate = dateTime; // Tutaj zrobić podział na update i Add !!!
                        var itemToCheck = _supplierRepo.GetAllIncom().FirstOrDefault(i => i.symbol_produktu == itemVm.symbol_produktu);
                        var mapedItemVm = _mapper.Map<Incom>(itemVm);
                        int itemId = 0;
                        if (itemToCheck != null && !string.IsNullOrEmpty(itemVm.ean))
                        {
                            mapedItemVm.createDate = itemToCheck.createDate;
                            mapedItemVm.updateTime = dateTime;
                            _supplierRepo.UpdateIncom(mapedItemVm);
                            UpdateItemInShop(mapedItemVm);
                            countItemUpdate++;
                            itemId = itemToCheck.Id;
                        }
                        if (itemToCheck == null && !string.IsNullOrEmpty(itemVm.ean))
                        {
                            itemId = _supplierRepo.AddIncomItem(mapedItemVm);
                            var result = ImageHelper.SaveImageFromUrl(linkImage, itemVm.ean, _webHost);
                            countImageAdd = countImageAdd + result;
                            countItemAdd++;
                        }
                        OmnibusPriceToListVm omnibusPrice = new OmnibusPriceToListVm();
                        var mapedOmnibusPrice = _mapper.Map<OmnibusPrice>(omnibusPrice);
                        mapedOmnibusPrice.PriceN = itemVm.cena;
                        mapedOmnibusPrice.Ean = itemVm.ean;
                        mapedOmnibusPrice.ChangeTime = dateTime;
                        mapedOmnibusPrice.WarehouseId = itemVm.warehouseId;
                        _omnibusPriceRepo.AddOmnibusPrice(mapedOmnibusPrice);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
            if (countItemAdd > 0 || countItemUpdate > 0)
            {
                returnRaport.raportAddItem.Add($"Usunięto rekordów: {countItemRemove}");
                returnRaport.raportAddItem.Add($"Dodano rekordów: {countItemAdd}");
                returnRaport.raportAddItem.Add($"Zmodyfikowano rekordów: {countItemUpdate}");
                returnRaport.raportAddItem.Add($"Dodano zdjęć: {countImageAdd}");
                returnRaport.raportAddItem.Add("ok");
            }
            returnRaport.warehouseForListVm = _itemRepo.GetAllWarehouses().ProjectTo<WarehouseForListVm>(_mapper.ConfigurationProvider).ToList();
            return returnRaport;
        }
        public AddIncomGroupsVm AddIncomGroupsXML(AddIncomGroupsVm incomGroups, XDocument xmlDocument)
        {
            int countItemAdd = 0;
            int countItemFalse = 0;
            int countItemRemove = 0;
            AddIncomGroupsVm returnRaport = new AddIncomGroupsVm();
            returnRaport.raportAddItem = new List<string>();
            if(incomGroups.removeItems == true)
            {
                _supplierRepo.DeleteIncomGroup();
            }
            foreach (XElement elementXml in xmlDocument.Root.Elements("grupy"))
            {
                XElement GroupId = elementXml.Element("id");
                XElement GroupIdHome = elementXml.Element("idh");
                XElement Name = elementXml.Element("name");

                IncomGroupForListVm incomGroupVm = new IncomGroupForListVm();
                incomGroupVm.GroupId = int.Parse(GroupId.Value);
                if (!string.IsNullOrEmpty(GroupIdHome.Value))
                {
                    incomGroupVm.GroupIdHome = int.Parse(GroupIdHome.Value);
                }
                else
                {
                    incomGroupVm.GroupIdHome = 0;
                }
                incomGroupVm.Name = Name.Value;
                var mappedIncomGroup = _mapper.Map<IncomGroup>(incomGroupVm);
                var returnId = _supplierRepo.AddIncomGroup(mappedIncomGroup);
                if (returnId != 0)
                {
                    countItemAdd++;
                }
                else
                {
                    countItemFalse++;
                }
            }
            returnRaport.raportAddItem.Add($"Dodano {countItemAdd} grup do bazy!");
            returnRaport.raportAddItem.Add($"Nie udało się dodać {countItemFalse} grup do bazy!");
            return returnRaport;
        }

        public int changeToInt(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                string resultString = text.Remove(text.Length - 4);
                //string cleanedText = new string(text.Where(char.IsDigit).ToArray());
                //decimal conToDecimal = decimal.Parse(cleanedText, cultureInfoDot);
                int result = int.Parse(resultString);
                return result;
            }
            return 0;
        }
        public decimal changeToDecimal(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                string resultString = text.Remove(text.Length - 1);
                resultString = resultString.Replace(".", ",");
                //string cleanedText = new string(text.Where(char.IsDigit).ToArray());
                var result = decimal.Parse(resultString);
                return result;
            }
            return 0;
        }

        public ConnectItemsToSupplierVm LoadConnectItemsToSupplierVm(int options)
        {
            ConnectItemsToSupplierVm newConnectItem = new ConnectItemsToSupplierVm();
            List<CountSupplierItem> listCountItem = new List<CountSupplierItem>();
            newConnectItem.raport = new List<string>();
            newConnectItem.groupItems = _groupItemRepo.GetAllGroupItem()
                .ProjectTo<GroupItemForListVm>(_mapper.ConfigurationProvider).ToList();
            newConnectItem.warehouseForLists = _itemRepo.GetAllWarehouses()
                     .ProjectTo<WarehouseForListVm>(_mapper.ConfigurationProvider).ToList();
            var _incomIdWarehouse = newConnectItem.warehouseForLists.FirstOrDefault(w => w.Name.Contains("Incom"));
            var _orinkIdWarehouse = newConnectItem.warehouseForLists.FirstOrDefault(w => w.Name.Contains("Orink"));
            int incomIdWarehouse = 0;
            int orinkIdWarehouse = 0;
            if (_incomIdWarehouse != null)
            {
                incomIdWarehouse = _incomIdWarehouse.Id;
            }
            if (_orinkIdWarehouse != null)
            {
                orinkIdWarehouse = _orinkIdWarehouse.Id;
            }
            if (options == incomIdWarehouse)
            {
                options = 1;
            }
            if (options == orinkIdWarehouse)
            {
                options = 2;
            }
            switch (options)
            {
                case 1:
                    newConnectItem.incomGroups = _supplierRepo.GetAllIncomGroup()
              .ProjectTo<IncomGroupForListVm>(_mapper.ConfigurationProvider).ToList();
                    foreach (var group in newConnectItem.incomGroups)
                    {
                        CountSupplierItem countItem = new CountSupplierItem();
                        if (group.GroupIdHome != null && group.GroupIdHome != 0)
                        {
                            var parrentName = newConnectItem.incomGroups.FirstOrDefault(g => g.GroupId == group.GroupIdHome).Name;
                            string groupName = $"{parrentName}->{group.Name}";
                            group.Name = groupName;
                        }
                        int count = _supplierRepo.GetAllIncom().Where(i => i.grupa_towarowa == group.GroupId.ToString()).Count();
                        if (count > 0)
                        {
                            countItem.groupId = group.GroupId;
                            countItem.countItem = count;
                            listCountItem.Add(countItem);
                        }
                    }
                    newConnectItem.countSupplierItems = listCountItem;
                    var ascendingList = newConnectItem.incomGroups.OrderBy(i => i.Name).ToList();
                    newConnectItem.incomGroups = ascendingList;
                    newConnectItem.selectedWarehouse = 3;
                    break;
                case 2:
                    newConnectItem.orinkGroups = _orinkRepo.GetAllorinkGroup()
                        .ProjectTo<OrinkGroupForListVm>(_mapper.ConfigurationProvider).ToList();
                    foreach (var group in newConnectItem.orinkGroups)
                    {
                        CountSupplierItem countItem = new CountSupplierItem();
                        int count = _orinkRepo.GetAllOrink().Where(i => i.OrinkGroupId == group.Id).Count();
                        if (count > 0)
                        {
                            countItem.groupId = group.Id;
                            countItem.countItem = count;
                            listCountItem.Add(countItem);
                        }
                    }
                    newConnectItem.countSupplierItems = listCountItem;
                    var ascendingListOrink = newConnectItem.orinkGroups.OrderBy(i => i.Name).ToList();
                    newConnectItem.orinkGroups = ascendingListOrink;
                    newConnectItem.selectedWarehouse = 4;
                    break;
                default:
                    break;
            }
            newConnectItem.categoryItems = _itemRepo.GetAllCategories()
                .ProjectTo<CategoryForListVm>(_mapper.ConfigurationProvider).ToList();
            foreach (var category in newConnectItem.categoryItems.Where(c => c.IsMainCategory == false))
            {
                var parrentCategory = newConnectItem.categoryItems.FirstOrDefault(c => c.Id == category.MainCategoryId);
                if (parrentCategory != null)
                {
                    string categoryName = $"{parrentCategory.Name}->{category.Name}";
                    category.Name = categoryName;
                }
            }
            var ascendingListCategory = newConnectItem.categoryItems.OrderBy(i => i.Name).ToList();
            newConnectItem.categoryItems = ascendingListCategory;
            newConnectItem.itemTagsForLists = _itemRepo.GetAllItemTags()
                .ProjectTo<ItemTagsForListVm>(_mapper.ConfigurationProvider).ToList();
            return newConnectItem;
        }

        public async Task<ConnectItemsToSupplierVm> AddConnectItemsToSupplierVm(ConnectItemsToSupplierVm connectedItems, int options)
        {
            ConnectItemsToSupplierVm newConnectItem = new ConnectItemsToSupplierVm();
            newConnectItem.warehouseForLists = _itemRepo.GetAllWarehouses()
                    .ProjectTo<WarehouseForListVm>(_mapper.ConfigurationProvider).ToList();
            newConnectItem.raport = new List<string>();
            //Dodawanie
            if (connectedItems.groupItem != null && !string.IsNullOrEmpty(connectedItems.groupItem.Name))
            {
                var mappedGroupItem = _mapper.Map<Domain.Model.GroupItem>(connectedItems.groupItem);
               await _groupItemRepo.AddGroupItemAsync(mappedGroupItem);
                string raport = $"Dodano pomyślnie grupę : {mappedGroupItem.Name}";
                newConnectItem.raport.Add(raport);
            }
            //Add Category 
            if ((connectedItems.category != null) && (!string.IsNullOrEmpty(connectedItems.category.Name)))
            {
                var mappedCategory = _mapper.Map<Category>(connectedItems.category);
                var idCategory =  await _itemRepo.AddCategoryAsync(mappedCategory);
                if (idCategory != 0)
                {
                    string raport = $"Dodano pomyślnie kategorię o nazwie: {mappedCategory.Name}";
                    newConnectItem.raport.Add(raport);
                }
            }
            //Add ItemTag
            if ((connectedItems.itemTag != null) && (!string.IsNullOrEmpty(connectedItems.itemTag.Name)))
            {
                var mappedItemTag = _mapper.Map<ItemTag>(connectedItems.itemTag);
                var idItemTag = await _itemRepo.AddItemTagAsync(mappedItemTag);
                if (idItemTag != 0)
                {
                    string raport = $"Dodano pomyślnie Tag o nazwie: {mappedItemTag.Name}";
                    newConnectItem.raport.Add(raport);
                }
            }
            if (options > 0)
            {
                var incomIdWarehouse = newConnectItem.warehouseForLists.FirstOrDefault(w => w.Name.Contains("Incom")).Id;
                var orinkIdWarehouse = newConnectItem.warehouseForLists.FirstOrDefault(w => w.Name.Contains("Orink")).Id;
                if (options == incomIdWarehouse)
                {
                    options = 1;
                }
                if (options == orinkIdWarehouse)
                {
                    options = 2;
                }
                switch (options)
                {
                    case 1:
                        if (connectedItems.selectedGroupSupplier > 0 && connectedItems.selectedCategory != null
                                        && connectedItems.selectedWarehouse != null)
                        {
                            var supplierItemByGroupId =_supplierRepo.GetAllIncom().Where(s => s.grupa_towarowa == connectedItems.selectedGroupSupplier.ToString()).ToList();
                            foreach (var supItem in supplierItemByGroupId)
                            {
                                var resultItem = _itemRepo.GetAllItems().FirstOrDefault(r => r.EanCode == supItem.ean);
                                int newItemId = 0;

                                if (resultItem == null && supItem.stan_magazynowy > 0)
                                {
                                    ItemForListVm newItem = new ItemForListVm();
                                    newItem.Name = supItem.nazwa_produktu;
                                    newItem.ItemSymbol = supItem.symbol_produktu;
                                    newItem.Description = supItem.opis;
                                    newItem.CategoryId = (int)connectedItems.selectedCategory;
                                    newItem.EanCode = supItem.ean;
                                    newItem.ImageFolder = supItem.ean;
                                    newItem.ProducentName = supItem.nazwa_producenta;
                                    newItem.Lenght = supItem.dlugosc;
                                    newItem.Width = supItem.szerokosc;
                                    newItem.Height = supItem.wysokosc;
                                    newItem.Weight = supItem.waga;
                                    newItem.GroupItemId = connectedItems.selectedGroupItem;
                                    var mappedNewItem = _mapper.Map<Item>(newItem);
                                    newItemId = _itemRepo.AddItem(mappedNewItem);
                                    string selectedVatRate = "A";
                                    var idVatRate = _itemRepo.GetAllVatRate().FirstOrDefault(i => i.Name == selectedVatRate);
                                    ItemWarehouseForListVm itemWarehouse = new ItemWarehouseForListVm();
                                    itemWarehouse.WarehouseId = (int)connectedItems.selectedWarehouse;
                                    itemWarehouse.ItemId = newItemId;
                                    itemWarehouse.Quantity = supItem.stan_magazynowy;
                                    itemWarehouse.NetPurchasePrice = supItem.cena;
                                    itemWarehouse.VatRateName = selectedVatRate;
                                    itemWarehouse.VatRateId = idVatRate.Id;
                                    var linkImage = supItem.urlImage;
                                    List<string> links = new List<string>();
                                    links.Add(linkImage);
                                    var mappedItemWare = _mapper.Map<ItemWarehouse>(itemWarehouse);
                                    //List<string> imgLink = new List<string>();
                                    //imgLink.Add(supItem.imgLink);
                                    var result = await ImageHelper.SaveImageFromUrlAsync(links, supItem.ean, _webHost);
                                    await _itemRepo.AddItemWarehouseAsync(mappedItemWare);
                                }
                                if (resultItem != null)
                                {
                                    ItemForListVm newItem = new ItemForListVm();
                                    newItemId = resultItem.Id;
                                    newItem.Id = newItemId;
                                    newItem.Name = supItem.nazwa_produktu;
                                    newItem.ItemSymbol = supItem.symbol_produktu;
                                    newItem.Description = supItem.opis;
                                    newItem.CategoryId = (int)connectedItems.selectedCategory;
                                    newItem.EanCode = supItem.ean;
                                    newItem.ImageFolder = supItem.ean;
                                    newItem.ProducentName = supItem.nazwa_producenta;
                                    newItem.Lenght = supItem.dlugosc;
                                    newItem.Width = supItem.szerokosc;
                                    newItem.Height = supItem.wysokosc;
                                    newItem.Weight = supItem.waga;
                                    newItem.GroupItemId = connectedItems.selectedGroupItem;
                                    var mappedNewItem = _mapper.Map<Item>(newItem);
                                   await _itemRepo.UpdateItemAsync(mappedNewItem);
                                }
                                if (connectedItems.selectedItemTags != null && connectedItems.selectedItemTags.Count > 0)
                                {
                                    foreach (var tag in connectedItems.selectedItemTags)
                                    {
                                        var sTag = _itemRepo.GetAllItemTags().FirstOrDefault(t => t.Id == tag);
                                        var resultItemTag = _itemRepo.GetAllConnectedItemTags().FirstOrDefault(r => r.ItemTagId == sTag.Id && r.ItemId == newItemId);
                                        if ((sTag != null) && ((int)connectedItems.selectedCategory > 0) && (newItemId != 0) && (resultItemTag == null))
                                        {
                                           await _itemRepo.AddConnectionItemTagsAsync(newItemId, _mapper.Map<ItemTag>(sTag));
                                            string raport = $"Dodano pomyślnie Tag o nazwie: {sTag.Name} do produktów z kategorii";
                                            newConnectItem.raport.Add(raport);
                                        }
                                    }
                                }
                            }
                            if (connectedItems.selectedItemTags != null && connectedItems.selectedItemTags.Count > 0)
                            {
                                foreach (var tag in connectedItems.selectedItemTags)
                                {
                                    var sTag = _itemRepo.GetAllItemTags().FirstOrDefault(t => t.Id == tag);
                                    var sCategory = _itemRepo.GetAllCategories().FirstOrDefault(c => c.Id == (int)connectedItems.selectedCategory);
                                    var resultTag = _categoryTagsRepo.GetAllCategoryTags().FirstOrDefault(r => r.ItemTagId == tag && r.CategoryId == sCategory.Id);
                                    if ((sTag != null) && ((int)connectedItems.selectedCategory > 0) && (resultTag == null))
                                    {
                                       await _categoryTagsRepo.AddConnectCategoryTagsAsync(sTag, (int)connectedItems.selectedCategory);
                                    }
                                }
                            }
                        }
                        break;
                    case 2:
                        if (connectedItems.selectedGroupSupplier > 0 && connectedItems.selectedCategory != null
                                      && connectedItems.selectedWarehouse == orinkIdWarehouse)
                        {
                            var supplierItemByGroupId = _orinkRepo.GetAllOrink().Where(s => s.OrinkGroupId == connectedItems.selectedGroupSupplier).ToList();
                            foreach (var supItem in supplierItemByGroupId)
                            {
                                var resultItem = _itemRepo.GetAllItems().FirstOrDefault(r => r.EanCode == supItem.ean);
                                int newItemId = 0;
                                if (resultItem == null && supItem.stan_magazynowy > 0)
                                {
                                    ItemForListVm newItem = new ItemForListVm();
                                    newItem.Name = supItem.name;
                                    newItem.ItemSymbol = supItem.symbol_producenta;
                                    newItem.Description = supItem.description;
                                    newItem.CategoryId = (int)connectedItems.selectedCategory;
                                    newItem.EanCode = supItem.ean;
                                    newItem.ImageFolder = "Orink";
                                    newItem.ProducentName = "Orink";
                                    newItem.Lenght = 0;
                                    newItem.Width = 0;
                                    newItem.Height = 0;
                                    newItem.Weight = 0;
                                    newItem.GroupItemId = connectedItems.selectedGroupItem;
                                    var mappedNewItem = _mapper.Map<Item>(newItem);
                                    newItemId = _itemRepo.AddItem(mappedNewItem);
                                    string selectedVatRate = "A";
                                    var idVatRate = _itemRepo.GetAllVatRate().FirstOrDefault(i => i.Name == selectedVatRate);
                                    ItemWarehouseForListVm itemWarehouse = new ItemWarehouseForListVm();
                                    itemWarehouse.WarehouseId = (int)connectedItems.selectedWarehouse;
                                    itemWarehouse.ItemId = newItemId;
                                    itemWarehouse.Quantity = supItem.stan_magazynowy;
                                    itemWarehouse.NetPurchasePrice = supItem.cenaNetto;
                                    itemWarehouse.VatRateName = selectedVatRate;
                                    itemWarehouse.VatRateId = idVatRate.Id;
                                    var mappedItemWare = _mapper.Map<ItemWarehouse>(itemWarehouse);
                                    //List<string> imgLink = new List<string>();
                                    //imgLink.Add(supItem.imgLink);
                                    //var result = ImageHelper.SaveOrinkImageFromUrl(imgLink, supItem.ean, _webHost, _httpClientFactory);
                                    _itemRepo.AddItemWarehouse(mappedItemWare);
                                }
                                if (resultItem != null)
                                {
                                    ItemForListVm newItem = new ItemForListVm();
                                    newItem.Id = resultItem.Id;
                                    newItem.Name = supItem.name;
                                    newItem.ItemSymbol = supItem.symbol_producenta;
                                    newItem.Description = supItem.description;
                                    newItem.CategoryId = (int)connectedItems.selectedCategory;
                                    newItem.EanCode = supItem.ean;
                                    newItem.ImageFolder = "Orink";
                                    newItem.ProducentName = "Orink";
                                    newItem.Lenght = 0;
                                    newItem.Width = 0;
                                    newItem.Height = 0;
                                    newItem.Weight = 0;
                                    newItem.GroupItemId = connectedItems.selectedGroupItem;
                                    var mappedNewItem = _mapper.Map<Item>(newItem);
                                    _itemRepo.UpdateItem(mappedNewItem);
                                    newItemId = resultItem.Id;
                                }
                                if (connectedItems.selectedItemTags != null && connectedItems.selectedItemTags.Count > 0)
                                {
                                    foreach (var tag in connectedItems.selectedItemTags)
                                    {
                                        var sTag = _itemRepo.GetAllItemTags().FirstOrDefault(t => t.Id == tag);
                                        var resultItemTag = _itemRepo.GetAllConnectedItemTags().FirstOrDefault(r => r.ItemTagId == sTag.Id && r.ItemId == newItemId);
                                        if ((sTag != null) && ((int)connectedItems.selectedCategory > 0) && (newItemId != 0) && (resultItemTag == null))
                                        {
                                            //  _categoryTagsRepo.AddConnectCategoryTags(sTag, (int)connectedItems.selectedCategory);

                                            _itemRepo.AddConnectionItemTags(newItemId, _mapper.Map<ItemTag>(sTag));
                                            string raport = $"Dodano pomyślnie Tag o nazwie: {sTag.Name} do produktów z kategorii";
                                            newConnectItem.raport.Add(raport);
                                        }
                                    }
                                }
                            }
                            if (connectedItems.selectedItemTags != null && connectedItems.selectedItemTags.Count > 0)
                            {
                                foreach (var tag in connectedItems.selectedItemTags)
                                {
                                    var sTag = _itemRepo.GetAllItemTags().FirstOrDefault(t => t.Id == tag);
                                    var sCategory = _itemRepo.GetAllCategories().FirstOrDefault(c => c.Id == (int)connectedItems.selectedCategory);
                                    var resultTag = _categoryTagsRepo.GetAllCategoryTags().FirstOrDefault(r => r.ItemTagId == tag && r.CategoryId == sCategory.Id);
                                    if ((sTag != null) && ((int)connectedItems.selectedCategory > 0) && (resultTag == null))
                                    {
                                        _categoryTagsRepo.AddConnectCategoryTags(sTag, (int)connectedItems.selectedCategory);
                                    }
                                }
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            //Add new or update item from supplier database


            return newConnectItem;
        }
        public bool UpdateItemInShop(Incom incomItems)
        {
            ItemWarehouse itemWarehouse = new ItemWarehouse();
            var itemShop = _itemRepo.GetAllItems().FirstOrDefault(i => i.EanCode == incomItems.ean);
            if (itemShop != null)
            {
                itemWarehouse = _itemRepo.GetAllItemWarehouses().FirstOrDefault(w => w.ItemId == itemShop.Id);
                if (itemWarehouse != null)
                {
                    try
                    {
                        itemWarehouse.NetPurchasePrice = incomItems.cena;
                        itemWarehouse.Quantity = incomItems.stan_magazynowy;
                        _itemRepo.UpdateItemWarehouse(itemWarehouse);
                        return true;
                    }
                    catch (Exception)
                    {
                        throw;
                    }

                }

            }
            return false;
        }
        public async Task<bool> UpdateItemInShopAsync(Incom incomItems)
        {
            ItemWarehouse itemWarehouse = new ItemWarehouse();
            var itemShop = _itemRepo.GetAllItems().FirstOrDefault(i => i.EanCode == incomItems.ean);
            if (itemShop != null)
            {
                itemWarehouse = _itemRepo.GetAllItemWarehouses().FirstOrDefault(w => w.ItemId == itemShop.Id);
                if (itemWarehouse != null)
                {
                    try
                    {
                        itemWarehouse.NetPurchasePrice = incomItems.cena;
                        itemWarehouse.Quantity = incomItems.stan_magazynowy;
                       await _itemRepo.UpdateItemWarehouseAsync(itemWarehouse);
                        return true;
                    }
                    catch (Exception)
                    {
                        throw;
                    }

                }

            }
            return false;
        }
    }
}
