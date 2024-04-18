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
        Task<IQueryable<Incom>> GetAllIncomAsync();
        void UpdateIncom(Incom incom);
        Task UpdateIncomAsync(Incom incom);
        void DeleteIncomItem(int incomId);
        Task DeleteIncomItemAsync(int incomId);
        int AddIncomGroup(IncomGroup incomGroup);
        void UpdateIncomGroup(IncomGroup incomGroup);
        IQueryable<IncomGroup> GetAllIncomGroup();
        void DeleteIncomGroup();
        IQueryable<ConnectCategoryGroup> GetAllConnectCategoryGroup();
        int AddConnectCategoryGroup(ConnectCategoryGroup connectCategoryGroup);
        void UpdateConnectCategoryGroup(ConnectCategoryGroup connectCategoryGroup);
        void DeleteConnectCategoryGroup(int id);
    }
}
