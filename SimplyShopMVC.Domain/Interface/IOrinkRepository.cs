using SimplyShopMVC.Domain.Model.Suppliers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Domain.Interface
{
    public interface IOrinkRepository
    {
        int AddOrinkItem(Orink orink);
        IQueryable<Orink> GetAllOrink();
        void UpdateOrink(Orink orink);
        void DeleteOrinkItem(int orinkId);
        int AddOrinkGroup(OrinkGroup orinkGroup);
        IQueryable<OrinkGroup> GetAllorinkGroup();
        void DeleteOrinkGroup(int orinkGroupId);
        void DeleteAllOrinkItem(IQueryable<Orink> orinks);
    }
}
