using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using SimplyShopMVC.Application.ViewModels.Front;
using SimplyShopMVC.Domain.Interface;
using SimplyShopMVC.Application.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimplyShopMVC.Domain.Model;

namespace SimplyShopMVC.Application.Services
{
    public class FrontService : IFrontService
    {
        private readonly ISupplierRepository _supplierRepo;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHost;
        private readonly IItemRepository _itemRepo;
        private readonly IGroupItemRepository _groupItemRepo;
        public FrontService(ISupplierRepository supplierRepo, IMapper mapper, IWebHostEnvironment webHost, IItemRepository itemRepo, IGroupItemRepository groupItemRepo)
        {
            _supplierRepo = supplierRepo;
            _mapper = mapper;
            _webHost = webHost;
            _itemRepo = itemRepo;
            _groupItemRepo = groupItemRepo;
        }

        public IndexListVm GetItemsToIndex(int? idItem, int? quantityItem) //DO PRZEMYŚLENIA - można dodać zmienne takie jak List<int>tagId, CategoryId, int przedmiotów do pobrania i obsłużyć jedną funkcją wszystkie strony sklepu 
        {
            IndexListVm indexList = new IndexListVm();

            List<FrontItemForList> frontItemForLists = new List<FrontItemForList>();
            List<Item> itemList = new List<Item>();
            do
            {
                if (idItem > 0 && idItem != null)
                {
                     itemList = _itemRepo.GetAllItems().Where
                }
                if (idItem == 0 || idItem == null)
                {
                     itemList = _itemRepo.GetAllItems().OrderBy(x => Guid.NewGuid()).Take(1).Where(i => i.IsActive == true && i.IsDeleted == false).ToList();
                }
                    
                foreach (var item in itemList)
                {
                    var checkDuplicateItem = frontItemForLists.FirstOrDefault(f => f.id == item.Id);
                    var indexItemWare = _itemRepo.GetAllItemWarehouses().FirstOrDefault(i => i.ItemId == item.Id);
                    if (indexItemWare != null && checkDuplicateItem == null)
                    {
                        var vatRateResoult = _itemRepo.GetAllVatRate().FirstOrDefault(v => v.Id == indexItemWare.VatRateId);  //Dodać sprawdzenie czy istnieje przedmiot w itemWArehouse!
                        FrontItemForList indexItem = new FrontItemForList();
                        indexItem.id = item.Id;
                        indexItem.name = item.Name;
                        indexItem.description = item.Description;
                        indexItem.eanCode = item.EanCode;
                        indexItem.imageFolder = item.ImageFolder;
                        indexItem.itemSymbol = item.ItemSymbol;
                        var groupIdresoult = _groupItemRepo.GetAllGroupItem().FirstOrDefault(g => g.Id == item.GroupItemId);
                        var warehouseResoult = _itemRepo.GetAllWarehouses().FirstOrDefault(w => w.Id == indexItemWare.WarehouseId);
                        //Można dodać sprawdzenie czy cena sprzedaży nie została nadana ręcznie w ItemWarehouse jeżeli nie to pobranie marży po grupie.              
                        var resultPriceB = GetPriceDetalB((decimal)indexItemWare.NetPurchasePrice.Value, vatRateResoult.Value, groupIdresoult.PriceMarkupA);
                        indexItem.priceB = resultPriceB;
                        indexItem.quantity = indexItemWare.Quantity;
                        indexItem.deliveryTime = warehouseResoult.DeliveryTime;
                        indexItem.imageUrl = ImageHelper.GetMainImageUrlFromPath(item.ImageFolder, _webHost);
                        frontItemForLists.Add(indexItem);
                    }
                }
            } while (frontItemForLists.Count <= 7);
            indexList.frontItemForLists = frontItemForLists;
            return indexList;
        }
        public decimal GetPriceDetalB(decimal netPurchasePrice, int vatRateValue, int groupMarkup)
        {
            decimal markupPercentage = (decimal)groupMarkup / 100;
            decimal vatPercentage = (decimal)vatRateValue / 100;

            // Wyliczenie ceny brutto
            decimal priceDetalB = netPurchasePrice * (1 + markupPercentage) * (1 + vatPercentage);

            return priceDetalB;
        }
    }
}
