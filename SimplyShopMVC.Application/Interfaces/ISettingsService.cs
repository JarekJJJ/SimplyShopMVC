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
        ListUserDetailForListVm UserSettings(ListUserDetailForListVm vm, int options);
        CompanySettingsVm EditCompanySettings(int flag, CompanySettingsVm? companyName);
    }
}
