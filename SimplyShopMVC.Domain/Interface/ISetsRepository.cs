using SimplyShopMVC.Domain.Model.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Domain.Interface
{
    public interface ISetsRepository
    {
        int AddPcSets(Cart cart);
        IQueryable<Cart> GetAllPcSets();
        void UpdatePcSets(Cart cart);
        void DeletePcSets(int cartId);
        int AddPcSetsItem(CartItems cartItem);
        IQueryable<CartItems> GetAllPcSetsItems();
        void UpdatePcSetsItem(CartItems cartItem);
        void DeletePcSetsItem(int cartItemId);
    }
}
