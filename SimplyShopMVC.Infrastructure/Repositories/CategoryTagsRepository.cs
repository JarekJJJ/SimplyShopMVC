﻿using Microsoft.EntityFrameworkCore;
using SimplyShopMVC.Domain.Interface;
using SimplyShopMVC.Domain.Model;
using SimplyShopMVC.Infrastructure.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Infrastructure.Repositories
{
    public class CategoryTagsRepository : ICategoryTagsRepository
    {
        private readonly Context _context;
        public CategoryTagsRepository(Context context)
        {
            _context = context;
        }
        public void AddConnectCategoryTags(ItemTag sTag, int CategoryId)
        {      
            var category = _context.Categories.FirstOrDefault(c=>c.Id == CategoryId);
                ConnectCategoryTag con = new ConnectCategoryTag();
                con.CategoryId = category.Id;
                con.ItemTagId = sTag.Id;
                _context.ConnectCategoryTags.Add(con);
                _context.SaveChanges();           
        }

        public IQueryable<ConnectCategoryTag> GetAllCategoryTags()
        {
            var result = _context.ConnectCategoryTags;
            return result;
        }
        public void DeleteConnectionCategoryTags(int CategoryId)
        {
            var result = _context.ConnectCategoryTags.Where(a => a.CategoryId == CategoryId);
            _context.ConnectCategoryTags.RemoveRange(result);
            _context.SaveChanges();
        }
    }
}
