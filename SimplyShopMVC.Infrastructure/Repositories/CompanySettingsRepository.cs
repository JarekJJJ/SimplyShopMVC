using SimplyShopMVC.Domain.Interface;
using SimplyShopMVC.Domain.Model.users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Infrastructure.Repositories
{
    public class CompanySettingsRepository : ICompanySettingsRepository
    {
        private readonly Context _context;
        public CompanySettingsRepository(Context context)
        {
            _context = context;
        }
        public int AddCompany(CompanySettings company)
        {
            var companySettings = _context.CompanySettings.FirstOrDefault();
            if (companySettings == null)
            {
                _context.Add(company);
                _context.SaveChanges();
                return company.Id;
            }
            else
            {
                return 0;
            }
           
        }

        public void DeleteCompany(int companyId)
        {
            throw new NotImplementedException();
        }

        public CompanySettings GetCompanySettings()
        {
            var company = _context.CompanySettings.FirstOrDefault();
            if (company != null)
            {
                return company;
            }
            else
            {
                CompanySettings companySettings = new CompanySettings();
                return companySettings;
            }
            
        }

        public void UpdateCompany(CompanySettings company)
        {
            _context.Update(company);
            _context.SaveChanges();
        }
    }
}
