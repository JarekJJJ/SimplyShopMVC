using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Hosting;
using SimplyShopMVC.Application.Helpers;
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
        private readonly IWebHostEnvironment _webHost;

        public ItemService(IItemRepository itemRepo, IMapper mapper, IWebHostEnvironment webHost)
        {
            _itemRepo = itemRepo;
            _mapper = mapper;
            _webHost = webHost;
        }

        public AddItemVm AddItem(AddItemVm item, IWebHostEnvironment webHostFolder) // Do wywalenia AddItemWarehouse !
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
            //if (item.selectedWarehouseId != null && item.ItemWarehouse.FinalPriceA != null && item.ItemWarehouse.Quantity != null)
            //{
            //    // var wItem = _mapper.Map<ItemWarehouse>(item.ItemWarehouse);
            //    ItemWarehouse wItem = new ItemWarehouse();

            //    wItem.WarehouseId = (int)item.selectedWarehouseId;
            //    wItem.ItemId = (int)mItem.Id;
            //    //wItem.VatRate = 23; // Dodać model oraz Vm ze stawkami (A-23%, B-8% ITD...)
            //    wItem.FinalPriceA = item.ItemWarehouse.FinalPriceA;
            //    wItem.Quantity = item.ItemWarehouse.Quantity;
            //    wItem.NetPurchasePrice = item.ItemWarehouse.NetPurchasePrice;
            //    _itemRepo.AddItemWarehouse(wItem);
            //}
            //List<Warehouse> warehouses = new List<Warehouse>();
            //var listWarehouse = _itemRepo.GetAllWarehouses()
            //    .ProjectTo<WarehouseForListVm>(_mapper.ConfigurationProvider).ToList();
            //item.warehouses = listWarehouse;

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
            var categoryMap = _mapper.Map<Category>(item.Category);
            //if (item.Category.Name != null)
            //{
            //    var category = new CategoryForListVm()
            //    {
            //        Id = item.CategoryId,
            //        Name = item.categoryName,
            //        Description = item.categoryDescription,
            //        IsActive = item.isActiveCategory,
            //        IsMainCategory = item.isMainCategory,
            //        MainCategoryId = item.mainCategoryId
            //    };

            var id = _itemRepo.AddCategory(categoryMap);
            return id;
        }

        public void DeleteItem(int id)
        {
            throw new NotImplementedException();
        }

        public UpdateItemVm GetItemToUpdate(int itemId)
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

        public AddItemWarehouseVm AddToUpdateItemWarehouse(int itemId)
        {
            var listItemWare = _itemRepo.GetAllItemWarehouses().Where(i => i.ItemId == itemId)
                 .ProjectTo<ItemWarehouseForListVm>(_mapper.ConfigurationProvider).ToList();
            var listWarehouse = _itemRepo.GetAllWarehouses()
                .ProjectTo<WarehouseForListVm>(_mapper.ConfigurationProvider).ToList();
            var listVatRate = _itemRepo.GetAllVatRate()
                .ProjectTo<VatRateForListVm>(_mapper.ConfigurationProvider).ToList();

            AddItemWarehouseVm itemWarehouseVm = new AddItemWarehouseVm();
            itemWarehouseVm.itemWarehouses = listItemWare;
            itemWarehouseVm.warehouses = listWarehouse;
            itemWarehouseVm.vatRate = listVatRate;
            itemWarehouseVm.itemId = itemId;
            return itemWarehouseVm;
        }
        public void AddItemWarehouse(AddItemWarehouseVm model)
        {
            var resultIW = _itemRepo.GetAllItemWarehouses().FirstOrDefault(i => i.ItemId == model.itemWarehouse.ItemId || i.WarehouseId == model.itemWarehouse.WarehouseId);
            if (resultIW != null)
            {
                var resultIWmapped = _mapper.Map<ItemWarehouse>(model.itemWarehouse);
                _itemRepo.UpdateItemWarehouse(resultIWmapped);
            }
            else
            {
                var modelMapped = _mapper.Map<ItemWarehouse>(model.itemWarehouse);
                _itemRepo.AddItemWarehouse(modelMapped);
            }



        }
        public AddItemWarehouseVm ListItemToUpdate(string searchItem)
        {
            AddItemWarehouseVm item = new AddItemWarehouseVm();
            if (searchItem != null)
            {
                var resultItem = _itemRepo.GetAllItems().Where(i => i.Name.Contains(searchItem) || i.EanCode.Contains(searchItem) || i.ItemSymbol.Contains(searchItem))
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

        public AddItemVm AddItemToUpdate(int selectedItem)
        {
            var item = _mapper.Map<AddItemVm>(_itemRepo.GetItemById(selectedItem));
            var newItemWare = _itemRepo.GetAllItemWarehouses().FirstOrDefault(i => i.ItemId == selectedItem);
            item.ItemTags = _itemRepo.GetAllItemTags()
                .ProjectTo<ItemTagsForListVm>(_mapper.ConfigurationProvider).ToList();
            var listItemTags = _itemRepo.GetConnectItemTags(item.Id).ToList();
            item.SelectedTags = new List<int>();
            foreach (var itemTag in listItemTags) //Lista przypisanych tagów to lista intów dlatego trzeba taką przygotować do widoku. 
            {
                item.SelectedTags.Add(itemTag.ItemTagId);
            }

            var _pathImage = $"{_webHost.WebRootPath}\\media\\itemimg\\{item.ImageFolder}\\";
            var imageToList = ImageHelper.AllImageFromPath(_pathImage).ToList();
            var listImage = new List<PhotoItemVm>();
            int photoId = 0;
            foreach (var imageUrl in imageToList)
            {
                var photoDetail = new PhotoItemVm();
                photoDetail.Id = photoId;
                photoId++;
                photoDetail.Name = imageUrl;
                var _imgFullUrl = $"/media/itemimg/{item.ImageFolder}/{imageUrl}";
                photoDetail.ImageUrl = _imgFullUrl;
                photoDetail.IsSelected = false;
                listImage.Add(photoDetail);
            }
            item.ListImages = listImage;
            //articleVm.SelectedTags = GetAllSelectedTagsForList(articleId);
            item.Categories = _itemRepo.GetAllCategories()
                .ProjectTo<CategoryForListVm>(_mapper.ConfigurationProvider).ToList();

            return item;
        }
        public int UpdateItem(AddItemVm model)
        {
            var _item = _mapper.Map<Item>(model);
            _item.CategoryId = model.selectedCategory;
            _itemRepo.UpdateItem(_item);
            string folderName;
            if (model.ImageFolder != null)
            {
                folderName = _item.ImageFolder;
            }
            else
            {
                _item.ImageFolder = _item.ItemSymbol;
                folderName = _item.ImageFolder;
            }
            string newFolderPath = Path.Combine(_webHost.WebRootPath, "media\\itemimg", folderName);
            try
            {
                Directory.CreateDirectory(newFolderPath);
            }
            catch (Exception)
            {

                throw;
            }

            if (model.Image != null)
            {
                foreach (var image in model.Image)
                {
                    string fileName = $"{image.FileName}";
                    string filePath = System.IO.Path.Combine(newFolderPath, fileName);
                    using (FileStream fileStream = System.IO.File.Create(filePath))
                    {
                        image.CopyTo(fileStream);
                        fileStream.Flush();
                    }
                }
            }
            if (model.SelectedImage != null)
            {
                foreach (var _image in model.SelectedImage)
                {
                    var result = ImageHelper.DeleteImage(_image, newFolderPath);
                }
            }
            AddTagsToItem(model.SelectedTags, model.Id);
            return _item.Id;
        }

        public UpdateItemTagVm ListItemTagToUpdate(string? searchTag)
        {
            UpdateItemTagVm updateItemTagVm = new UpdateItemTagVm();
            updateItemTagVm.countTag = new List<CountItemTagVm>();

            if (searchTag == null)
            {
                var listItemTags = _itemRepo.GetAllItemTags().ProjectTo<ItemTagsForListVm>(_mapper.ConfigurationProvider).Take(20).ToList();
                updateItemTagVm.itemTags = listItemTags;
                foreach (var tags in updateItemTagVm.itemTags)
                {
                    CountItemTagVm countTag = new CountItemTagVm();
                    var count = _itemRepo.GetItemsByTagId(tags.Id).Count();
                    countTag.countItem = count;
                    countTag.itemTagId = tags.Id;

                    updateItemTagVm.countTag.Add(countTag);
                }
                return updateItemTagVm;
            }

            updateItemTagVm.itemTags = _itemRepo.GetAllItemTags().Where(t => t.Name.Contains(searchTag))
                .ProjectTo<ItemTagsForListVm>(_mapper.ConfigurationProvider).ToList();
            return updateItemTagVm;
        }

        public UpdateItemTagVm GetItemTagToUpdate(int tagId)
        {
            UpdateItemTagVm updateItemTag = new UpdateItemTagVm();
            var result = _itemRepo.GetItemTagByTagId(tagId);
            var tagMap = _mapper.Map<ItemTagsForListVm>(result);
            updateItemTag.itemTag = tagMap;
            return updateItemTag;
        }

        public void UpdateItemTag(UpdateItemTagVm itemTag, int options)
        {
            if (itemTag != null && options == 1)
            {
                var mapItemTag = _mapper.Map<ItemTag>(itemTag.itemTag);
                _itemRepo.UpdateItemTag(mapItemTag);
            }
            if (itemTag != null && options == 2)
            {
                _itemRepo.DeleteItemTag(itemTag.itemTag.Id);
            }
            if (itemTag != null && options == 3)
            {
                var mapItemTag = _mapper.Map<ItemTag>(itemTag.itemTag);
                _itemRepo.AddItemTag(mapItemTag);
            }
        }
    }
}
