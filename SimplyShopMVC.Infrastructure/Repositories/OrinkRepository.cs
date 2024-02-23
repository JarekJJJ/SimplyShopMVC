using SimplyShopMVC.Domain.Interface;
using SimplyShopMVC.Domain.Model.Suppliers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Infrastructure.Repositories
{
    public class OrinkRepository : IOrinkRepository
    {
        private readonly Context _context;
        public OrinkRepository(Context context)
        {
            _context = context;
        }
        public int AddOrinkGroup(OrinkGroup orinkGroup)
        {
            _context.OrinkGroups.Add(orinkGroup);
            _context.SaveChanges();
            return orinkGroup.Id;
        }

        public int AddOrinkItem(Orink orink)
        {
            _context.Orinks.Add(orink);
            _context.SaveChanges();
            return orink.Id;
        }

        public void DeleteOrinkGroup(int orinkGroupId)
        {
            throw new NotImplementedException();
        }

        public void DeleteOrinkItem(int orinkId)
        {
            var result = _context.Orinks.FirstOrDefault(i=>i.Id==orinkId);
            if(result!=null)
            {
                _context.Orinks.Remove(result);
                _context.SaveChanges();
            }
        }

        public IQueryable<Orink> GetAllOrink()
        {
            var result = _context.Orinks;
            return result;
        }

        public IQueryable<OrinkGroup> GetAllorinkGroup()
        {
            var result = _context.OrinkGroups;
            return result;
        }

        public void UpdateOrink(Orink orink)
        {
            _context.Orinks.Update(orink);
            _context.SaveChanges();
        }
    }
}
