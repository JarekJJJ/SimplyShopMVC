using FluentValidation.Validators;
using SimplyShopMVC.Application.Interfaces;
using SimplyShopMVC.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Application.Helpers
{
    public class PriceCalculate : IPriceCalculate
    {
        private readonly IUserRepository _userRepo;
        private readonly IItemRepository _itemRepo;
        private readonly IGroupItemRepository _groupItemRepo;
        public PriceCalculate(IUserRepository userRepo, IItemRepository itemRepo, IGroupItemRepository groupItemRepo)
        {
            _userRepo = userRepo;
            _itemRepo = itemRepo;
            _groupItemRepo = groupItemRepo;
        }

        public decimal priceCalc(int itemId,int warehouseId, string userId)
        {
            var userDetail = _userRepo.GetAllUsers().FirstOrDefault(u => u.UserId == userId);
            var itemWareDetail = _itemRepo.GetAllItemWarehouses().FirstOrDefault(i => i.ItemId == itemId && i.WarehouseId == warehouseId);
            var itemDetail = _itemRepo.GetAllItems().FirstOrDefault(it => it.Id == itemId);
            var groupDetail = _groupItemRepo.GetAllGroupItem().FirstOrDefault(g => g.Id == itemDetail.GroupItemId);
            var vatRateResoult = _itemRepo.GetAllVatRate().FirstOrDefault(v=>v.Id==itemWareDetail.VatRateId);
            var resultPriceLevelA = GetPriceDetalB((decimal)itemWareDetail.NetPurchasePrice.Value, vatRateResoult.Value, groupDetail.PriceMarkupA);
            var resultPriceLevelB = GetPriceDetalB((decimal)itemWareDetail.NetPurchasePrice.Value, vatRateResoult.Value, groupDetail.PriceMarkupB);
            var resultPriceLevelC = GetPriceDetalB((decimal)itemWareDetail.NetPurchasePrice.Value, vatRateResoult.Value, groupDetail.PriceMarkupC);
            decimal price = 0;
            if (userDetail == null)
            {
                price = resultPriceLevelA;
            }
            else
            {
                switch (userDetail.PriceLevel)
                {
                    case ("A"):
                        price = resultPriceLevelA;
                        break;
                    case ("B"):
                        price = resultPriceLevelB;
                        break;
                    case ("C"):
                        price = resultPriceLevelC;
                        break;
                    default:
                        price = resultPriceLevelA;
                        break;
                }
            }
            return price;
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
