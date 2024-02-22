using AutoMapper;
using AutoMapper.QueryableExtensions;
using SimplyShopMVC.Application.Interfaces;
using SimplyShopMVC.Application.ViewModels.user;
using SimplyShopMVC.Domain.Interface;
using SimplyShopMVC.Domain.Model.users;
using SimplyShopMVC.Infrastructure.Migrations;
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
        public void AddUserSettings(string userId, string userName)
        {
            var userSeetings = _userRepo.GetAllUsers().Where(x => x.UserId == userId);
            if(!userSeetings.Any())
            {
                UserDetailForListVm userDetail= new UserDetailForListVm();
                userDetail.UserId = userId;
                userDetail.EmailAddress = userName;
                var mappedUserDetail = _mapper.Map<UserDetail>(userDetail);
                _userRepo.AddUserDetail(mappedUserDetail);
            }
        }
        public ListUserDetailForListVm UserSettings(ListUserDetailForListVm vm, int options, string searchString)
        {
            ListUserDetailForListVm listUserDetail = new ListUserDetailForListVm();
           
            switch (options)
            {
                case 1: // pobieranie danych użytkownika
                    listUserDetail.listUserDetail = _userRepo.GetAllUsers()
                       .ProjectTo<UserDetailForListVm>(_mapper.ConfigurationProvider).OrderByDescending(a => a.Id).Take(100).ToList();
                    listUserDetail.userDetail = _mapper.Map<UserDetailForListVm>(_userRepo.GetAllUsers().FirstOrDefault(u=>u.Id == vm.userDetail.Id));
                    return listUserDetail;
                case 2: // Zapis danych użytkownika
                    _userRepo.UpdateUserDetail(_mapper.Map<UserDetail>(vm.userDetail));
                    listUserDetail.listUserDetail = _userRepo.GetAllUsers()
                       .ProjectTo<UserDetailForListVm>(_mapper.ConfigurationProvider).OrderByDescending(a => a.Id).Take(100).ToList();
                    return listUserDetail;
                case 3:// Wyszukiwanie użytkownika
                    listUserDetail.listUserDetail = _userRepo.GetAllUsers().Where(u=>u.FullName.Contains(searchString) || u.NIP.Contains(searchString) || u.EmailAddress.Contains(searchString))
                        .ProjectTo<UserDetailForListVm>(_mapper.ConfigurationProvider).OrderByDescending(a => a.Id).Take(100).ToList();
                    break;
                default:
                    listUserDetail.listUserDetail = _userRepo.GetAllUsers().Where(u => u.FullName.Contains(searchString) || u.NIP.Contains(searchString) || u.EmailAddress.Contains(searchString))
                       .ProjectTo<UserDetailForListVm>(_mapper.ConfigurationProvider).OrderByDescending(a => a.Id).Take(100).ToList();
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
