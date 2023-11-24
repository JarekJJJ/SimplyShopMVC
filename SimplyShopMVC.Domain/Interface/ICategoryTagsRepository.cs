using SimplyShopMVC.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Domain.Interface
{
    // W CategoryTag wyorzystywane są te same tagi co w ItemTag - służą do filtrowania przedmiotów w kategoriach.
    public interface ICategoryTagsRepository
    {
       void AddConnectCategoryTags(int ItemTagId, int CategoryId);
       ItemTag GetTagsByCategoryId(int CategoryId);
    }
}
