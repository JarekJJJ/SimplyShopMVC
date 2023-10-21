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

        public AddItemVm AddItem(AddItemVm item, IWebHostEnvironment webHostFolder) //Dodać automatyczny symbol !!!
        {
            var mItem = _mapper.Map<Item>(item);
            if (mItem.Name != null)
            {
                List<ItemTagsForListVm> tags = new List<ItemTagsForListVm>();
                string folderName;
                if (mItem.EanCode != null)
                {
                    folderName = mItem.EanCode;
                    mItem.ImageFolder = folderName;
                }
                else
                {
                    DateTime now = DateTime.Now;
                    folderName = now.ToString("yyyyMMddHHmmss");
                    mItem.ImageFolder = folderName;
                }
                if (mItem.ItemSymbol == null)
                {
                    mItem.ItemSymbol = DateTime.Now.ToString("yyyyMMddHHmmss");
                }
                mItem.CategoryId = item.selectedCategory;
                var id = _itemRepo.AddItem(mItem);
                string newFolderPath = Path.Combine(webHostFolder.WebRootPath, "media\\itemimg", folderName);
                Directory.CreateDirectory(newFolderPath);
                if (item.Image != null)
                {
                    foreach (var file in item.Image)
                    {
                        string fileName = $"{file.FileName}";
                        string filePath = System.IO.Path.Combine(newFolderPath, fileName);
                        using (FileStream fileStream = System.IO.File.Create(filePath))
                        {
                            file.CopyTo(fileStream);
                            fileStream.Flush();
                        }
                    }
                }
                AddTagsToItem(item.SelectedTags, id);
            }

            List<ItemTagsForListVm> listTags = new List<ItemTagsForListVm>();
            var allTags = _itemRepo.GetAllItemTags()
                .ProjectTo<ItemTagsForListVm>(_mapper.ConfigurationProvider).ToList();
            foreach (var tag in allTags)
            {
                listTags.Add(tag);
            }
            item.ItemTags = listTags;

            List<CategoryForListVm> listCategory = new List<CategoryForListVm>();
            var allCategories = _itemRepo.GetAllCategories()
                .ProjectTo<CategoryForListVm>(_mapper.ConfigurationProvider).ToList();
            foreach (var category in allCategories)
            {
                listCategory.Add(category);
            }
            item.Categories = listCategory;
            if (item.selectedWarehouseId != null && item.ItemWarehouse.FinalPriceA != null && item.ItemWarehouse.Quantity != null)
            {
               // var wItem = _mapper.Map<ItemWarehouse>(item.ItemWarehouse);
               ItemWarehouse wItem = new ItemWarehouse();
               
                wItem.WarehouseId = (int)item.selectedWarehouseId;
                wItem.ItemId = (int)mItem.Id;
                wItem.VatRate = 23; // Dodać model oraz Vm ze stawkami (A-23%, B-8% ITD...)
                wItem.FinalPriceA = item.ItemWarehouse.FinalPriceA;
                wItem.Quantity = item.ItemWarehouse.Quantity;
                wItem.NetPurchasePrice = item.ItemWarehouse.NetPurchasePrice;
                _itemRepo.AddItemWarehouse(wItem);
            }
            List<Warehouse> warehouses = new List<Warehouse>();
            var listWarehouse = _itemRepo.GetAllWarehouses()
                .ProjectTo<WarehouseForListVm>(_mapper.ConfigurationProvider).ToList();
            item.warehouses = listWarehouse;

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
                    Description = item.TagDescription
                };
                var tagMap = _mapper.Map<ItemTag>(tags);
                var id = _itemRepo.AddItemTag(tagMap);
                return id;
            }
            return 0;
        }
        public int AddCategory(AddItemVm item)
        {
            if (item.categoryName != null)
            {
                var category = new CategoryForListVm()
                {
                    Id = item.CategoryId,
                    Name = item.categoryName,
                    Description = item.categoryDescription,
                    IsActive = item.isActiveCategory,
                    IsMainCategory = item.isMainCategory,
                    MainCategoryId = item.mainCategoryId
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
        public void AddTagsToItem(List<int> tags, int itemId)
        {
            _itemRepo.DeleteConnectionItemTags(itemId);
            foreach (var stags in tags)
            {
                var element = _itemRepo.GetItemTagByTagId(stags);
                _itemRepo.AddConnectionItemTags(itemId, element);
            }
        }

        public AddItemWarehouseVm AddItemWarehouse(AddItemWarehouseVm item)
        {
            if (item.searchItem != null) // dodać szukanie po ean !!!
            {

            }
            return item;
        }
        public AddItemWarehouseVm ListItemToUpdate(string searchItem)
        {
            AddItemWarehouseVm item = new AddItemWarehouseVm();
            if (searchItem != null)
            {
                var resultItem = _itemRepo.GetAllItems().Where(i => i.Name.Contains(searchItem))
                       .ProjectTo<ItemForListVm>(_mapper.ConfigurationProvider).Take(20);
                item.items = resultItem.OrderByDescending(i => i.Id).ToList();
            }
            else
            {
                var resultItem = _itemRepo.GetAllItems()
                    .ProjectTo<ItemForListVm>(_mapper.ConfigurationProvider).Take(20);
                var resultItemWare = _itemRepo.GetAllItemWarehouses()
                    .ProjectTo<ItemWarehouseForListVm>(_mapper.ConfigurationProvider).Take(20);
                item.items = resultItem.OrderByDescending(i => i.Id).ToList();
                item.itemWarehouses = resultItemWare.ToList();
            }
            return item;
        }
    }
}
