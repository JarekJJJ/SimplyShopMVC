using SimplyShopMVC.Domain.Interface;
using SimplyShopMVC.Domain.Model.Order;
using SimplyShopMVC.Domain.Model.Sets;
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

        public int AddPcSets(PcSets set)
        {
            _context.PcSets.Add(set);
            _context.SaveChanges();
            return set.Id;
        }

        public int AddPcSetsItem(PcSetsItems setItem)
        {
            _context.PcSetsItems.Add(setItem);
            _context.SaveChanges();
            return setItem.Id;
        }

        public void DeletePcSets(int setId)
        {
            var pcset = _context.PcSets.FirstOrDefault(i=>i.Id == setId);
            if (pcset != null)
            {
                _context.PcSets.Remove(pcset);
                _context.SaveChanges();
            }
        }

        public void DeletePcSetsItem(int setItemId)
        {
           var setItem = _context.PcSetsItems.FirstOrDefault(i=>i.Id == setItemId);
            if(setItem!=null)
            {
                _context.PcSetsItems.Remove(setItem);
                _context.SaveChanges();
            }
        }

        public IQueryable<PcSets> GetAllPcSets()
        {
          var pcsets =  _context.PcSets;
            return pcsets;
        }

        public IQueryable<PcSetsItems> GetAllPcSetsItems()
        {
           return _context.PcSetsItems;
        }

        public void UpdatePcSets(PcSets set)
        {
            _context.Update(set);
            _context.SaveChanges();
        }

        public void UpdatePcSetsItem(PcSetsItems setItem)
        {
           _context.Update(setItem);
            _context.SaveChanges();
        }
    }
}
