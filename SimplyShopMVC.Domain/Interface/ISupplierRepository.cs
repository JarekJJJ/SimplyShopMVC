using SimplyShopMVC.Domain.Model.Suppliers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Domain.Interface
{
    public interface ISupplierRepository
    {
        // Incom supplier
        int AddIncomItem(Incom incom);
        IQueryable<Incom> GetAllIncom();
        void UpdateIncom(Incom incom);
        void DeleteIncomItem(int incomId);
    }
}
