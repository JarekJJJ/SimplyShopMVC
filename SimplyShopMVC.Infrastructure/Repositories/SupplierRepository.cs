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

        public int AddIncomItem(Incom incom)
        {
            _context.Add(incom);
            _context.SaveChanges();
            return incom.Id;
        }

        public void DeleteIncomItem(int incomId)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Incom> GetAllIncom()
        {
            throw new NotImplementedException();
        }

        public void UpdateIncom(Incom incom)
        {
            throw new NotImplementedException();
        }
    }
}
