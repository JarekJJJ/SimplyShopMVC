using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Hosting;
using SimplyShopMVC.Application.Interfaces;
using SimplyShopMVC.Application.ViewModels.Article;
using SimplyShopMVC.Application.ViewModels.Item;
using SimplyShopMVC.Domain.Interface;
using SimplyShopMVC.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Application.Services
{
    public class ItemService : IItemService
    {
        private readonly IItemRepository _itemRepo;
        private readonly IMapper _mapper;

        public ItemService(IItemRepository itemRepo, IMapper mapper)
        {
            _itemRepo = itemRepo;
            _mapper = mapper;
        }

        public AddItemVm AddItem(AddItemVm item, IWebHostEnvironment webHostFolder)
        {
            var mItem = _mapper.Map<Item>(item);
            if (mItem.Name != null)
            {
                List<ItemTagsForListVm> tags = new List<ItemTagsForListVm>();
                var id = _itemRepo.AddItem(mItem);
                if (mItem.EanCode != null)
                {
                    var folderName = mItem.EanCode;
                } // Tutaj dokończyć --------- Dodawanie item !
            }
           
                List<ItemTagsForListVm> listTags = new List<ItemTagsForListVm>();
                var allTags = _itemRepo.GetAllItemTags()
                    .ProjectTo<ItemTagsForListVm>(_mapper.ConfigurationProvider).ToList();
                foreach (var tag in allTags)
                {
                    listTags.Add(tag);
                }
                item.ItemTags = listTags;
           //to samo z kategoriami !
           
                List<CategoryForListVm> listCategory = new List<CategoryForListVm>();
                var allCategories = _itemRepo.GetAllCategories()
                    .ProjectTo<CategoryForListVm>(_mapper.ConfigurationProvider).ToList();
                foreach (var category in allCategories)
                {
                    listCategory.Add(category);
                }
                item.Categories = listCategory;
            

            return item;
        }

        public int AddItemTag(AddItemVm item)
        {
            var listTags = _itemRepo.GetAllItemTags().Where(t => t.Name.Equals(item.TagName)).Count();
            if (item.TagName != null && listTags == 0)
            {
                var tags = new ItemTagsForListVm()
                {
                    Id = item.TagId,
                    Name = item.TagName,
                    Description= item.TagDescription
                };
                var tagMap = _mapper.Map<ItemTag>(tags);
                var id = _itemRepo.AddItemTag(tagMap);
                return id;
            }
            return 0;
        }
        public int AddCategory(AddItemVm item)
        {
            if(item.categoryName!=null)
            {
                var category = new CategoryForListVm()
                {
                    Id = item.CategoryId,
                    Name = item.categoryName,
                    Description = item.categoryDescription,
                    IsActive= item.isActiveCategory,
                    IsMainCategory = item.isMainCategory,
                    MainCategoryId= item.mainCategoryId
                };
                var categoryMap = _mapper.Map<Category>(category);
                var id = _itemRepo.AddCategory(categoryMap);
                return id;
            }
            return 0;
        }

        public void DeleteItem(int id)
        {
            throw new NotImplementedException();
        }

        public UpdateItemVm GetItemToUpdate(int itemId)
        {
            throw new NotImplementedException();
        }

        public void UpdateItem(UpdateItemVm item, List<string> selectedImage)
        {
            throw new NotImplementedException();
        }
    }
}
