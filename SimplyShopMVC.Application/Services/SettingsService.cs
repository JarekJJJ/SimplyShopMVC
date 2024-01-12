using AutoMapper;
using SimplyShopMVC.Application.Interfaces;
using SimplyShopMVC.Application.ViewModels.user;
using SimplyShopMVC.Domain.Interface;
using SimplyShopMVC.Domain.Model.users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Application.Services
{
    public class SettingsService : ISettingsService
    {
        private readonly ICompanySettingsRepository _companySettingsRepo;
        private readonly IMapper _mapper;
        public SettingsService(ICompanySettingsRepository companySettingsRepo, IMapper mapper)
        {
            _companySettingsRepo = companySettingsRepo;
            _mapper = mapper;
        }

        public CompanySettingsVm EditCompanySettings(int flag, CompanySettingsVm? companyName)
        {
           if(flag == 1 && companyName != null)
            {
               var companyId =  _companySettingsRepo.AddCompany(_mapper.Map<CompanySettings>(companyName));
                if(companyId== 0 )
                {
                    var comapny =_mapper.Map<CompanySettingsVm>(_companySettingsRepo.GetCompanySettings());
                    return comapny;
                }
                return companyName;
            }
            if(flag == 2 && companyName != null)
            {
                _companySettingsRepo.UpdateCompany(_mapper.Map<CompanySettings>(companyName));
                return companyName;
            }
            CompanySettingsVm newCompanyName= new CompanySettingsVm();
            return newCompanyName; //zrobić kontroler i widok dodawania / edycji danych sprzedawcy.
        }
    }
}
