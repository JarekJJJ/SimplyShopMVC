using AutoMapper;
using AutoMapper.QueryableExtensions;
using SimplyShopMVC.Application.Interfaces;
using SimplyShopMVC.Application.ViewModels.Item;
using SimplyShopMVC.Domain.Interface;
using SimplyShopMVC.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Application.Helpers
{
    public class OmnibusHelper : IOmnibusHelper
    {
        private readonly IOmnibusPriceRepository _priceRepo;
        private readonly IItemRepository _itemRepo;
        private readonly IMapper _mapper;
        private readonly IGroupItemRepository _groupItemRepo;

        public OmnibusHelper(IOmnibusPriceRepository priceRepo, IItemRepository itemRepo, IMapper mapper, IGroupItemRepository groupItemRepo)
        {
            _priceRepo = priceRepo;
            _itemRepo = itemRepo;
            _mapper = mapper;
            _groupItemRepo = groupItemRepo;
        }

        public void AddPriceToHistory(decimal priceN, string eanCode, int warehouseId)
        {
            OmnibusPriceToListVm omnibusPrice = new OmnibusPriceToListVm();
            if (priceN > 0 && !string.IsNullOrEmpty(eanCode) && warehouseId > 0)
            {
                omnibusPrice.PriceN = priceN;
                omnibusPrice.Ean = eanCode;
                omnibusPrice.WarehouseId = warehouseId;
                omnibusPrice.ChangeTime = DateTime.Now;
                var mappedObject = _mapper.Map<OmnibusPrice>(omnibusPrice);
                _priceRepo.AddOmnibusPrice(mappedObject);
            }
        }
        public void AddPriceToHistory(ItemWarehouseForListVm item)
        {
            OmnibusPriceToListVm omnibusPrice = new OmnibusPriceToListVm();
            var itemEan = _itemRepo.GetItemById(item.ItemId);
            if (item != null && item.NetPurchasePrice > 0 && !string.IsNullOrEmpty(itemEan.EanCode))
            {
                omnibusPrice.PriceN = (decimal)item.NetPurchasePrice;
                omnibusPrice.Ean = itemEan.EanCode;
                omnibusPrice.WarehouseId = item.WarehouseId;
                omnibusPrice.ChangeTime = DateTime.Now;
                var mappedObject = _mapper.Map<OmnibusPrice>(omnibusPrice);
                _priceRepo.AddOmnibusPrice(mappedObject);
            }
        }
        public List<OmnibusPriceToListVm> GetOmnibusPrice(string eanCode)
        {
            List<OmnibusPriceToListVm> omnibusPriceList = new List<OmnibusPriceToListVm>();
            var oPriceList = _priceRepo.GetAllOmnibusPrice().Where(i => i.Ean == eanCode)
                .ProjectTo<OmnibusPriceToListVm>(_mapper.ConfigurationProvider).ToList();
            omnibusPriceList.AddRange(oPriceList);
            //var item = _itemRepo.GetAllItems().FirstOrDefault(i => i.EanCode == eanCode);
            //var itemW = _itemRepo.GetAllItemWarehouses().FirstOrDefault(i => i.ItemId == item.Id);
            //var vatValue = _itemRepo.GetAllVatRate().FirstOrDefault(i => i.Id == itemW.VatRateId);
            //var groupItem = _groupItemRepo.GetAllGroupItem().FirstOrDefault(i => i.Id == item.GroupItemId);
            //if (item != null && groupItem != null)
            //{
            //    foreach (var oPrice in omnibusPriceList)
            //    {
            //        var a = (oPrice.PriceN * (((decimal)groupItem.PriceMarkupA / 100) + 1))*(((decimal)vatValue.Value/100)+1);
            //        oPrice.PriceDetB = a;
            //    }
            //}
            return omnibusPriceList;
        }
        public List<OmnibusPriceToListVm> GetOmnibusPrice(ItemForListVm item)
        {
            List<OmnibusPriceToListVm> omnibusPriceList = new List<OmnibusPriceToListVm>();
            var oPriceList = _priceRepo.GetAllOmnibusPrice().Where(i => i.Ean == item.EanCode)
                .ProjectTo<OmnibusPriceToListVm>(_mapper.ConfigurationProvider).ToList();
            omnibusPriceList.AddRange(oPriceList);
            var groupItem = _groupItemRepo.GetAllGroupItem().FirstOrDefault(i => i.Id == item.GroupItemId);
            if (item != null && groupItem != null)
            {
                foreach (var oPrice in omnibusPriceList)
                {
                    var a = oPrice.PriceDetB * ((decimal)groupItem.PriceMarkupA / 100) + 1;
                    oPrice.PriceDetB = a;
                }
            }
            return omnibusPriceList;
        }
        public List<OmnibusPriceToListVm> GetAllOmnibusPrice(string eanCode, int numberMonths)
        {
            List<OmnibusPriceToListVm> listOmnibusPrice = new List<OmnibusPriceToListVm>();
            var listAllPrice = _priceRepo.GetAllOmnibusPrice().Where(i => i.Ean == eanCode).ToList();           
           for(var i = 1; i <= numberMonths;i++)
            {
                var lastTime = DateTime.Now.AddMonths(-i+1);
                var actualTime = DateTime.Now.AddMonths(-i);
                var tempListPrice = listAllPrice.Where(x => (x.ChangeTime >= actualTime && x.ChangeTime<lastTime)).OrderBy(p => p.PriceN).Take(1).ToList();
                var tempPrice =_mapper.Map<OmnibusPriceToListVm>(tempListPrice.FirstOrDefault());
                if(tempPrice != null)
                {
                    listOmnibusPrice.Add(tempPrice);
                }              
            }
            return listOmnibusPrice;
        }
    }
}
