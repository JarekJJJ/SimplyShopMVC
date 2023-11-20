using SimplyShopMVC.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Domain.Interface
{
    public interface IOmnibusPriceRepository
    {
        int AddOmnibusPrice(OmnibusPrice omnibusPrice);
        void UpdateOmnibusPrice(OmnibusPrice omnibusPrice);
        IQueryable<OmnibusPrice> GetAllOmnibusPrice();
        void DeleteOmnibusPrice(int id);
    }
}
