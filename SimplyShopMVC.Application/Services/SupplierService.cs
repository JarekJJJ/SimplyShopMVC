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

        public SupplierService(IItemRepository itemRepo, IMapper mapper, IWebHostEnvironment webHost, ISupplierRepository supplierRepo, IGroupItemRepository groupItemRepo, IOmnibusPriceRepository omnibusPriceRepo, ICategoryTagsRepository categoryTagsRepo)
        {
            _itemRepo = itemRepo;
            _mapper = mapper;
            _webHost = webHost;
            _supplierRepo = supplierRepo;
            _groupItemRepo = groupItemRepo;
            _omnibusPriceRepo = omnibusPriceRepo;
            _categoryTagsRepo = categoryTagsRepo;
        }
        public AddIncomItemsVm UpdateIncomItemsXML(AddIncomItemsVm incomItems, XDocument xmlDocument)
        {
            int countItemUpdate = 0;
            int countItemAdd = 0;
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
                            mapedItemVm.updateTime= dateTime;
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

        public ConnectItemsToSupplierVm LoadConnectItemsToSupplierVm()
        {
            ConnectItemsToSupplierVm newConnectItem = new ConnectItemsToSupplierVm();
            List<CountSupplierItem> listCountItem = new List<CountSupplierItem>();
            newConnectItem.raport = new List<string>();
            newConnectItem.groupItems = _groupItemRepo.GetAllGroupItem()
                .ProjectTo<GroupItemForListVm>(_mapper.ConfigurationProvider).ToList();
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
            newConnectItem.warehouseForLists = _itemRepo.GetAllWarehouses()
                    .ProjectTo<WarehouseForListVm>(_mapper.ConfigurationProvider).ToList();
            newConnectItem.itemTagsForLists = _itemRepo.GetAllItemTags()
                .ProjectTo<ItemTagsForListVm>(_mapper.ConfigurationProvider).ToList();
            return newConnectItem;
        }

        public ConnectItemsToSupplierVm AddConnectItemsToSupplierVm(ConnectItemsToSupplierVm connectedItems)
        {
            ConnectItemsToSupplierVm newConnectItem = new ConnectItemsToSupplierVm();
            newConnectItem.raport = new List<string>();
            //Dodawanie
            if (connectedItems.groupItem != null && !string.IsNullOrEmpty(connectedItems.groupItem.Name))
            {
                var mappedGroupItem = _mapper.Map<GroupItem>(connectedItems.groupItem);
                _groupItemRepo.AddGroupItem(mappedGroupItem);
                string raport = $"Dodano pomyślnie grupę : {mappedGroupItem.Name}";
                newConnectItem.raport.Add(raport);
            }
            //Add Category 
            if ((connectedItems.category != null) && (!string.IsNullOrEmpty(connectedItems.category.Name)))
            {
                var mappedCategory = _mapper.Map<Category>(connectedItems.category);
                var idCategory = _itemRepo.AddCategory(mappedCategory);
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
                var idItemTag = _itemRepo.AddItemTag(mappedItemTag);
                if (idItemTag != 0)
                {
                    string raport = $"Dodano pomyślnie Tag o nazwie: {mappedItemTag.Name}";
                    newConnectItem.raport.Add(raport);
                }
            }
            //Add new or update item from supplier database
            if (connectedItems.selectedGroupSupplier > 0 && connectedItems.selectedCategory != null
                && connectedItems.selectedWarehouse != null)
            {
                var supplierItemByGroupId = _supplierRepo.GetAllIncom().Where(s => s.grupa_towarowa == connectedItems.selectedGroupSupplier.ToString()).ToList();
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
                        var mappedItemWare = _mapper.Map<ItemWarehouse>(itemWarehouse);
                        _itemRepo.AddItemWarehouse(mappedItemWare);
                    }
                    if (resultItem != null)
                    {
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
    }
}
