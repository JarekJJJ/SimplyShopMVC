using SimplyShopMVC.Domain.Interface;
using SimplyShopMVC.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Infrastructure.Repositories
{
    public class OmnibusPriceRepository : IOmnibusPriceRepository
    {
        private readonly Context _context;
        public OmnibusPriceRepository(Context context)
        {
            _context = context;
        }
        public int AddOmnibusPrice(OmnibusPrice omnibusPrice)
        {
            var id = _context.Add(omnibusPrice);
            _context.SaveChanges();
            return omnibusPrice.Id;

        }
        public async Task<int> AddOmnibusPriceAsync(OmnibusPrice omnibusPrice)
        {
            var id = _context.Add(omnibusPrice);
            await _context.SaveChangesAsync();
            return omnibusPrice.Id;

        }

        public void DeleteOmnibusPrice(int id)
        {
            var element = _context.OmnibusPrices.Find(id);
            if (element != null)
            {
                _context.Remove(element);
                _context.SaveChanges();
            }

        }

        public IQueryable<OmnibusPrice> GetAllOmnibusPrice()
        {
            var elements = _context.OmnibusPrices;
            return elements;
        }

        public void UpdateOmnibusPrice(OmnibusPrice omnibusPrice)
        {
            _context.Update(omnibusPrice);
        }
    }
}
