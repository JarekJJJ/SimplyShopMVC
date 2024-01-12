using SimplyShopMVC.Domain.Model.users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Domain.Interface
{
    public interface ICompanySettingsRepository
    {
        int AddCompany(CompanySettings company);
        void UpdateCompany(CompanySettings company);
        void DeleteCompany(int companyId);
        CompanySettings GetCompanySettings();

    }
}
