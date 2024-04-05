using SimplyShopMVC.Domain.Model.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Domain.Interface
{
    public interface IDeliveryRepository
    {
        Task<int> AddDeliveryAsync(Delivery delivery);
        int AddDelivery(Delivery delivery);
        Task UpdateDeliveryAsync(Delivery delivery);
        IQueryable<Delivery> GetAllDeliveries();
        void DeleteDelivery(int deliveryId);
    }
}
