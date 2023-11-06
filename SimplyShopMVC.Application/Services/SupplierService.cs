using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using SimplyShopMVC.Application.Interfaces;
using SimplyShopMVC.Application.ViewModels.Suppliers;
using SimplyShopMVC.Domain.Interface;
using SimplyShopMVC.Domain.Model.Suppliers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace SimplyShopMVC.Application.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly ISupplierRepository _supplierRepo;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHost;
        private readonly IItemRepository _itemRepo;

        public SupplierService(IItemRepository itemRepo, IMapper mapper, IWebHostEnvironment webHost, ISupplierRepository supplierRepo)
        {
            _itemRepo = itemRepo;
            _mapper = mapper;
            _webHost = webHost;
            _supplierRepo = supplierRepo;
        }
        public string LoadIncomItemsXML(XDocument xmlDocument)
        {
            CultureInfo cultureInfo = new CultureInfo("pl-PL");
            cultureInfo.NumberFormat.NumberDecimalSeparator = ",";

            DateTime dateTime= DateTime.Now;

            foreach (XElement elementXml in xmlDocument.Root.Elements("produkt"))
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
                foreach (XElement parametrOpisu in opisProduktu.Elements("parametr"))
                {
                    string paramName = parametrOpisu.Element("nazwa")?.Value;
                    string paramValue = parametrOpisu.Element("wartosc")?.Value;
                    string newLineDescription = $"<tr><td>{paramName}</td><td>{paramValue}</td></tr>";
                    lineDescription = lineDescription + newLineDescription;
                }
                string fullDescription = firstLineDescription + lineDescription + lastLineDescription;
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
                itemVm.warehouseId = 3; // ToDo dodać wybór magazynu przy dodawaniu pliku xml w formularzu !
                itemVm.grupa_towarowa = grupa_towarowa.Value;
                itemVm.nazwa_grupy_towarowej = nazwa_grupy_towarowej.Value;
                itemVm.symbol_produktu = symbol_produktu.Value;
                itemVm.nazwa_produktu = nazwa_produktu.Value;
                itemVm.symbol_producenta = symbol_producenta.Value;
                itemVm.ean = ean.Value;
                itemVm.nazwa_producenta = nazwa_producenta.Value;
                itemVm.opis = fullDescription;
                //string cleanedText = new string(stan_magazynowy.Value.Where(char.IsDigit).ToArray());                             
                itemVm.stan_magazynowy = changeToInt(stan_magazynowy.Value);
                itemVm.cena = decimal.Parse(cena.Value, cultureInfo);
                itemVm.dlugosc = changeToDecimal(dlugosc.Value);
                itemVm.szeroksc = changeToDecimal(szerokosc.Value);
                itemVm.wysokosc = changeToDecimal(wysokosc.Value);
                itemVm.waga = changeToDecimal(waga.Value);
                itemVm.createDate = dateTime; // Tutaj zrobić podział na update i Add !!!
                var mapedItemVm = _mapper.Map<Incom>(itemVm);
                var itemId = _supplierRepo.AddIncomItem(mapedItemVm);

            }
            return "ok";

        }

        //        if (ProductName != null && XEanCode != null)
        //        {
        //            NewItemVm itemVm = new NewItemVm();
        //            NewWarehouseItemVm newWarehouseItemVm = new NewWarehouseItemVm();
        //            int warehouseItemId = 0;

        //            var validationItem = _itemRepo.GetAllItems().FirstOrDefault(i => i.EanCode == XEanCode.Value);
        //            if (validationItem == null)
        //            {
        //                itemVm.Name = ProductName.Value;
        //                itemVm.Symbol = ProductSymbol.Value;
        //                itemVm.ImageFolderName = XEanCode.Value;
        //                itemVm.EanCode = XEanCode.Value;
        //                itemVm.ShortDescription = ProductDescription.Value;
        //                itemVm.Producent = ProducentName.Value;
        //                itemVm.IsDeleted = false;
        //                itemVm.IsActive = false;
        //                SaveImageFromLink(ImageLink.Value, itemVm.EanCode);
        //                var newItem = _mapper.Map<Item>(itemVm);
        //                var id = _itemRepo.AddItem(newItem);

        //                warehouseItemId = id; //jeżeli produkt nie istnieje pobierane jest Id z nowo utworzonego
        //            }
        //            else
        //            {
        //                warehouseItemId = validationItem.Id; //Jeżeli isnieje pobierane jest z istniejącego
        //            }
        //            var warehouseItem = _wItemRepo.GetItem(itemVm.Id, 1);

        //            if (warehouseItem == null)
        //            {
        //                newWarehouseItemVm.SuppCategoryId = int.Parse(suppCategory.Value);
        //                newWarehouseItemVm.ItemId = warehouseItemId;
        //                newWarehouseItemVm.WarehouseId = 1;
        //                newWarehouseItemVm.VatRate = 23;
        //                newWarehouseItemVm.Quantity = int.Parse(ProductQuantity.Value);
        //                newWarehouseItemVm.NetPurchasePrice = float.Parse(PurchasePrice.Value, cultureInfo);
        //                newWarehouseItemVm.IsActive = false;
        //                var _newWarehouseItem = _mapper.Map<WarehouseItem>(newWarehouseItemVm);
        //                var wId = _wItemRepo.AddNewDelivery(_newWarehouseItem);

        //            }
        //            else
        //            {
        //                //newWarehouseItemVm.Quantity = int.Parse(ProductQuantity.Value);
        //                //newWarehouseItemVm.NetPurchasePrice = float.Parse(PurchasePrice.Value);
        //                //var _newWarehouseItem = _mapper.Map<WarehouseItem>(newWarehouseItemVm);
        //                //var wId = _wItemRepo.UpdateItemInWarehouse(_newWarehouseItem);

        //            }


        //            // _itemRepo.AddItem - zrobić mapowanie oraz zapis do warehouseItem
        //        }

        //    }
        //}
        public int changeToInt(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                string resultString = text.Remove(text.Length-4);
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
    }
    
}
