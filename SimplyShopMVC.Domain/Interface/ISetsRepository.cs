using SimplyShopMVC.Domain.Model.Order;
using SimplyShopMVC.Domain.Model.Sets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Domain.Interface
{
    public interface ISetsRepository
    {
        int AddPcSets(PcSets set);
        IQueryable<PcSets> GetAllPcSets();
        void UpdatePcSets(PcSets set);
        void DeletePcSets(int setId);
        int AddPcSetsItem(PcSetsItems setItem);
        IQueryable<PcSetsItems> GetAllPcSetsItems();
        void UpdatePcSetsItem(PcSetsItems setItem);
        void DeletePcSetsItem(int setItemId);
    }
}
