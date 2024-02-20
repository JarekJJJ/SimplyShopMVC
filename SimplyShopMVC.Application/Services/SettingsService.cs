using AutoMapper;
using AutoMapper.QueryableExtensions;
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
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;
        public SettingsService(ICompanySettingsRepository companySettingsRepo, IMapper mapper, IUserRepository userRepo)
        {
            _companySettingsRepo = companySettingsRepo;
            _mapper = mapper;
            _userRepo = userRepo;
        }

        public ListUserDetailForListVm UserSettings(ListUserDetailForListVm vm, int options)
        {
            ListUserDetailForListVm listUserDetail = new ListUserDetailForListVm();
            switch (options)
            {
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
                default:
                    listUserDetail.listUserDetail = _userRepo.GetAllUsers()
                        .ProjectTo<UserDetailForListVm>(_mapper.ConfigurationProvider).ToList();
                    return listUserDetail;
                  
            }
            return listUserDetail;
        }

        public CompanySettingsVm EditCompanySettings(int flag, CompanySettingsVm? companyName)
        {
            if (flag == 1 && companyName.Id == 0)
            {
                var companyId = _companySettingsRepo.AddCompany(_mapper.Map<CompanySettings>(companyName));
                if (companyId == 0)
                {
                    var comapny = _mapper.Map<CompanySettingsVm>(_companySettingsRepo.GetCompanySettings());
                    comapny.resultInfo = "Dane firmy już wprowadzono można tylko edytować dane !";
                    return comapny;
                }
                var companyAdd = _mapper.Map<CompanySettingsVm>(_companySettingsRepo.GetCompanySettings());
                companyAdd.resultInfo = "Pomyślnie wprowadzono dane!";
                return companyAdd;
            }
            if (companyName.Id != 0)
            {
                _companySettingsRepo.UpdateCompany(_mapper.Map<CompanySettings>(companyName));
                var companyAdd = _mapper.Map<CompanySettingsVm>(_companySettingsRepo.GetCompanySettings());
                companyAdd.resultInfo = "Pomyślnie poprawiono dane!";
                return companyAdd;
            }
            if (flag == 0)
            {
                var comapny = _mapper.Map<CompanySettingsVm>(_companySettingsRepo.GetCompanySettings());
                if (!string.IsNullOrEmpty(comapny.CompanyName))
                {
                    return comapny;
                }
                else
                {
                    CompanySettingsVm newCompanyName = new CompanySettingsVm();
                    return newCompanyName;
                }
            }
            return companyName;
        }
    }
}
