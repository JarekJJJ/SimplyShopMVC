using SimplyShopMVC.Domain.Model.users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Domain.Interface
{
    public interface IUserRepository
    {
        int AddUserDetail(UserDetail userDetail);
        void UpdateUserDetail(UserDetail userDetail);
        void DeleteUserDetail(string userId);
        IQueryable<UserDetail> GetAllUsers();
    }
}
