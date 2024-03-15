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
        Task<int> AddIncomItemAsync(Incom incom);
        IQueryable<Incom> GetAllIncom();
        Task<List<Incom>> GetAllIncomAsync();
        void UpdateIncom(Incom incom);
        Task UpdateIncomAsync(Incom incom);
        void DeleteIncomItem(int incomId);
        Task DeleteIncomItemAsync(int incomId);
        int AddIncomGroup(IncomGroup incomGroup);
        IQueryable<IncomGroup> GetAllIncomGroup();
        void DeleteIncomGroup();
    }
}
