﻿using Microsoft.EntityFrameworkCore;
using SimplyShopMVC.Domain.Interface;
using SimplyShopMVC.Domain.Model.users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly Context _context;
        public UserRepository(Context context)
        {
            _context = context;
        }
        public int AddUserDetail(UserDetail userDetail)
        {
            _context.Add(userDetail);
            _context.SaveChanges();
            return userDetail.Id;
        }

        public void DeleteUserDetail(string userId)
        {
            var userDetailToRemove = _context.UserDetails.Find(userId);
            if (userDetailToRemove != null)
            {
                _context.Remove(userDetailToRemove);
                _context.SaveChanges();
            }
        }

        public IQueryable<UserDetail> GetAllUsers()
        {
           var userDetail =  _context.UserDetails;
            return userDetail;
        }

        public void UpdateUserDetail(UserDetail userDetail)
        {
            var existingUser = _context.UserDetails.Find(userDetail.Id);

            if (existingUser != null)
            {
                _context.Entry(existingUser).State = EntityState.Detached; // Odłącz istniejącego użytkownika
                _context.Attach(userDetail); // Dołącz nowego użytkownika
                _context.Entry(userDetail).State = EntityState.Modified; // Oznacz jako zmodyfikowany
                _context.SaveChanges();
            }
        }
    }
}
