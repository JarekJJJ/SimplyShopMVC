using SimplyShopMVC.Domain.Interface;
using SimplyShopMVC.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Infrastructure.Repositories
{
    public class CategoryTagsRepository : ICategoryTagsRepository
    {
        public void AddConnectCategoryTags(int ItemTagId, int CategoryId)
        {
            throw new NotImplementedException();
        }

        public ItemTag GetTagsByCategoryId(int CategoryId)
        {
            throw new NotImplementedException();
        }
    }
}
