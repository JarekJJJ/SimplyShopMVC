using Microsoft.IdentityModel.Tokens;
using SimplyShopMVC.Domain.Interface;
using SimplyShopMVC.Domain.Model.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Infrastructure.Repositories
{
    public class DeliveryRepository : IDeliveryRepository
    {
        private readonly Context _context;
        public DeliveryRepository(Context context)
        {
            _context = context;
        }

        public async Task<int> AddDeliveryAsync(Delivery delivery)
        {
            _context.Delivery.Add(delivery);
           var id = await _context.SaveChangesAsync();
            return id;
        }
        public int AddDelivery(Delivery delivery)
        {
            _context.Delivery.Add(delivery);
            var id =  _context.SaveChanges();
            return id;
        }
        public void DeleteDelivery(int deliveryId)
        {
           var result = _context.Delivery.FirstOrDefault(d=>d.Id== deliveryId);
            if (result != null)
            {
                _context.Delivery.Remove(result);
                _context.SaveChanges();
            }
        }

        public IQueryable<Delivery> GetAllDeliveries()
        {
            var result = _context.Delivery;
            return result;
        }

        public async Task UpdateDeliveryAsync(Delivery delivery)
        {
            _context.Delivery.Update(delivery);
            await _context.SaveChangesAsync();
        }
    }
}
