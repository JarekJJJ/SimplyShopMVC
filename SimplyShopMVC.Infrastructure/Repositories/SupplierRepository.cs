using Microsoft.EntityFrameworkCore;
using SimplyShopMVC.Domain.Interface;
using SimplyShopMVC.Domain.Model.Suppliers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Infrastructure.Repositories
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly Context _context;
        public SupplierRepository(Context context)
        {
            _context = context;
        }   
        public IQueryable<ConnectCategoryGroup> GetAllConnectCategoryGroup()
        {
            var result = _context.ConnectCategoryGroups;
            return result;
        }
        public int AddConnectCategoryGroup(ConnectCategoryGroup connectCategoryGroup)
        {
            _context.Add(connectCategoryGroup);
            _context.SaveChanges();
            return connectCategoryGroup.Id;
        }
        public void UpdateConnectCategoryGroup(ConnectCategoryGroup connectCategoryGroup)
        {
            _context.Update(connectCategoryGroup);
            _context.SaveChanges();
        }
        public void DeleteConnectCategoryGroup(int id)
        {
            var result = _context.ConnectCategoryGroups.FirstOrDefault(c => c.Id == id);
            if (result != null)
            {
                _context.Remove(result);
                _context.SaveChanges();
            }        
        }
        public int AddIncomItem(Incom incom)
        {
            _context.Add(incom);
            _context.SaveChanges();
            return incom.Id;
        }
        public async Task<int> AddIncomItemAsync(Incom incom)
        {
            _context.Add(incom);
            await _context.SaveChangesAsync();
            return incom.Id;
        }


        public void DeleteIncomItem(int incomId)
        {
            var result = _context.Incoms.FirstOrDefault(x => x.Id== incomId);
            _context.Remove(result);
            _context.SaveChanges();
        }
        public async Task DeleteIncomItemAsync(int incomId)
        {
            var result = _context.Incoms.FirstOrDefault(x => x.Id == incomId);
            _context.Remove(result);
            await _context.SaveChangesAsync();
        }

        public IQueryable<Incom> GetAllIncom()
        {
            var result = _context.Incoms;
            return result;
        }
        public async Task<IQueryable<Incom>> GetAllIncomAsync()
        {
            var result = _context.Incoms;
            return result;
        }
        public void UpdateIncom(Incom incom)
        {
            var incomItem = _context.Incoms.FirstOrDefault(i => i.symbol_produktu == incom.symbol_produktu);
            incom.Id = incomItem.Id;
            _context.Entry(incomItem).State = EntityState.Detached;
            _context.Update(incom);
            _context.SaveChanges();
        }
        public async Task UpdateIncomAsync(Incom incom)
        {
            var incomItem = _context.Incoms.FirstOrDefault(i => i.symbol_produktu == incom.symbol_produktu);
            incom.Id = incomItem.Id;
            _context.Entry(incomItem).State = EntityState.Detached;
            _context.Update(incom);
           await _context.SaveChangesAsync();
        }
        public int AddIncomGroup(IncomGroup incomGroup)
        {
            _context.Add(incomGroup);
            _context.SaveChanges();
            return incomGroup.Id;
        }
        public void UpdateIncomGroup(IncomGroup incomGroup)
        {
            _context.Update(incomGroup);
            _context.SaveChanges();
        }
        public IQueryable<IncomGroup> GetAllIncomGroup()
        {
           var result = _context.IncomGroups;
            return result;
        }
        public void DeleteIncomGroup() //Usuwanie wszystkich grup
        {
            var result = _context.IncomGroups;
            _context.RemoveRange(result);
            _context.SaveChanges();
        }

    }
}
