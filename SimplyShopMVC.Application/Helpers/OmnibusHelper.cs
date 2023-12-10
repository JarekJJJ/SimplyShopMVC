using AutoMapper;
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
    public class OmnibusHelper
    {
        private readonly IOmnibusPriceRepository _priceRepo;
        private readonly IItemRepository _itemRepo;
        private readonly IMapper _mapper;

        public OmnibusHelper(IOmnibusPriceRepository priceRepo, IItemRepository itemRepo, IMapper mapper)
        {
            _priceRepo = priceRepo;
            _itemRepo = itemRepo;
            _mapper = mapper;
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
            return omnibusPriceList;
        }
    }
}
