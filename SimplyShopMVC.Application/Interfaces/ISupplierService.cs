using SimplyShopMVC.Application.ViewModels.Suppliers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SimplyShopMVC.Application.Interfaces
{
    public interface ISupplierService
    {
        ListConnectingCategoryVm GetConnectCategoryWithSupplierGroup();
        void DeleteConnectCategoryWithSupplierGroup(ListConnectingCategoryVm result);
        void AddConnectCategoryWithSupplierGroup(ListConnectingCategoryVm result);
        AddIncomItemsVm LoadNewIncomGroupForView();
        Task<AddIncomItemsVm> LoadNewIncomItemsXML(AddIncomItemsVm incomItems, XDocument xmlDocument);
        AddIncomItemsVm LoadOrinkItemsXML(AddIncomItemsVm orinkItems, XDocument xmlDocument);
        AddIncomItemsVm UpdateIncomItemsXML(AddIncomItemsVm incomItems, XDocument xmlDocument);
        Task<AddIncomItemsVm> UpdateIncomItemsXMLAsync(AddIncomItemsVm incomItems, XDocument xmlDocument);
        AddIncomItemsVm LoadIncomItemsXML(AddIncomItemsVm incomItems, XDocument xmlDocument);
        AddIncomGroupsVm AddIncomGroupsXML(AddIncomGroupsVm incomGroups, XDocument xmlDocument);
        ConnectItemsToSupplierVm LoadConnectItemsToSupplierVm(int options);
        Task<ConnectItemsToSupplierVm> AddConnectItemsToSupplierVm(ConnectItemsToSupplierVm connectedItems, int options);
    }
}
