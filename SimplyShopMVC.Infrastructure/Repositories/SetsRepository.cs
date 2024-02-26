using SimplyShopMVC.Domain.Interface;
using SimplyShopMVC.Domain.Model.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Infrastructure.Repositories
{
    public class SetsRepository : ISetsRepository
    {
        private readonly Context _context;
        public SetsRepository(Context context)
        {
            _context = context;
        }
        public int AddPcSets(Cart cart)
        {
            throw new NotImplementedException();
        }

        public int AddPcSetsItem(CartItems cartItem)
        {
            throw new NotImplementedException();
        }

        public void DeletePcSets(int cartId)
        {
            throw new NotImplementedException();
        }

        public void DeletePcSetsItem(int cartItemId)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Cart> GetAllPcSets()
        {
            throw new NotImplementedException();
        }

        public IQueryable<CartItems> GetAllPcSetsItems()
        {
            throw new NotImplementedException();
        }

        public void UpdatePcSets(Cart cart)
        {
            throw new NotImplementedException();
        }

        public void UpdatePcSetsItem(CartItems cartItem)
        {
            throw new NotImplementedException();
        }
    }
}
