using SimplyShopMVC.Application.ViewModels.user;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Application.Interfaces
{
    public interface ISettingsService
    {
        void AddUserSettings(string userId, string userName);
        ListUserDetailForListVm UserSettings(ListUserDetailForListVm vm, int options, string searchString);
        CompanySettingsVm EditCompanySettings(int flag, CompanySettingsVm? companyName);
    }
}
