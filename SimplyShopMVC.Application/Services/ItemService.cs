using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
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
        private readonly IGroupItemRepository _groupItemRepo;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHost;

        public ItemService(IItemRepository itemRepo, IMapper mapper, IWebHostEnvironment webHost, IGroupItemRepository groupItemRepo)
        {
            _itemRepo = itemRepo;
            _mapper = mapper;
            _webHost = webHost;
            _groupItemRepo = groupItemRepo;
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
        public AddItemWarehouseVm AddItemWarehouse(AddItemWarehouseVm model)
        {
            AddItemWarehouseVm result = new AddItemWarehouseVm();
            var resultIW = _itemRepo.GetAllItemWarehouses().FirstOrDefault(i => i.ItemId == model.itemWarehouse.ItemId && i.WarehouseId == model.itemWarehouse.WarehouseId);
            if (resultIW != null)
            {
                resultIW.Quantity = model.itemWarehouse.Quantity;
                resultIW.NetPurchasePrice = model.itemWarehouse.NetPurchasePrice;
                resultIW.VatRateId = model.itemWarehouse.VatRateId;
                resultIW.VatRateName = _itemRepo.GetAllVatRate().FirstOrDefault(i => i.Id == model.itemWarehouse.VatRateId).Name;
                //var resultIWmapped = _mapper.Map<ItemWarehouse>(model.itemWarehouse);

                _itemRepo.UpdateItemWarehouse(resultIW);
            }
            else
            {
                var modelMapped = _mapper.Map<ItemWarehouse>(model.itemWarehouse);
                _itemRepo.AddItemWarehouse(modelMapped);
            }
            result.itemWarehouse = _mapper.Map<ItemWarehouseForListVm>(resultIW);
            result.itemWarehouses = _itemRepo.GetAllItemWarehouses().Where(a => a.ItemId == model.itemWarehouse.ItemId)
                .ProjectTo<ItemWarehouseForListVm>(_mapper.ConfigurationProvider).ToList();
            result.warehouses = _itemRepo.GetAllWarehouses().ProjectTo<WarehouseForListVm>(_mapper.ConfigurationProvider).ToList();
            result.vatRate = _itemRepo.GetAllVatRate().ProjectTo<VatRateForListVm>(_mapper.ConfigurationProvider).ToList();
            return result;
        }
        public AddItemWarehouseVm ListItemToUpdate(string searchItem, int? countItem) //Lista artykułów - Panel administratora - wczytywanie listy do edycji
        {
            if (countItem == 0 || countItem == null)
            {
                countItem = 20;
            }
            AddItemWarehouseVm item = new AddItemWarehouseVm();
            if (searchItem != null)
            {
                var resultItem = _itemRepo.GetAllItems().Where(i => i.Name.Contains(searchItem) || i.EanCode.Contains(searchItem) || i.ItemSymbol.Contains(searchItem))
                       .ProjectTo<ItemForListVm>(_mapper.ConfigurationProvider).Take((int)countItem);
                item.items = resultItem.OrderByDescending(i => i.Id).ToList();
            }
            else
            {
                var resultItem = _itemRepo.GetAllItems()
                    .ProjectTo<ItemForListVm>(_mapper.ConfigurationProvider).Take((int)countItem);
                item.items = resultItem.OrderByDescending(i => i.Id).ToList();
            }
            var resultItemWare = _itemRepo.GetAllItemWarehouses()
                   .ProjectTo<ItemWarehouseForListVm>(_mapper.ConfigurationProvider);
            item.itemWarehouses = resultItemWare.ToList();
            item.warehouses = _itemRepo.GetAllWarehouses().ProjectTo<WarehouseForListVm>(_mapper.ConfigurationProvider).ToList();
            item.itemTags = _itemRepo.GetAllItemTags().ProjectTo<ItemTagsForListVm>(_mapper.ConfigurationProvider).ToList();
            //item.categories = _itemRepo.GetAllCategories().ProjectTo<CategoryForListVm>(_mapper.ConfigurationProvider).ToList();
            List<ItemTagsForListVm> tempListTag = new List<ItemTagsForListVm>();
            foreach (var _item in item.items) // dodawanie tagów występujących w wybranych produktach
            {
                var listTags = _itemRepo.GetAllConnectedItemTags().Where(i => i.ItemId == _item.Id);
                if (listTags.Any())
                {
                    foreach (var tag in listTags)
                    {
                        var tempTag = _mapper.Map<ItemTagsForListVm>(_itemRepo.GetAllItemTags().FirstOrDefault(i => i.Id == tag.ItemTagId));
                        var resultTag = tempListTag.FirstOrDefault(i => i.Id == tempTag.Id);
                        if (resultTag == null)
                        {
                            tempListTag.Add(tempTag);
                        }
                    }
                }
            }
            item.forDeleteItemTags = tempListTag.ToList();
            item.categories = _itemRepo.GetAllCategories()
                .ProjectTo<CategoryForListVm>(_mapper.ConfigurationProvider).ToList();
            foreach (var category in item.categories.Where(c => c.IsMainCategory == false))
            {
                var parrentCategory = item.categories.FirstOrDefault(c => c.Id == category.MainCategoryId);
                if (parrentCategory != null)
                {
                    string categoryName = $"{parrentCategory.Name}->{category.Name}";
                    category.Name = categoryName;
                }

            }
            var ascendingListCategory = item.categories.OrderBy(i => i.Name).ToList();
            item.categories = ascendingListCategory;
            return item;
        }
        public List<string> UpdateItemFromList(AddItemWarehouseVm listItem)
        {
            List<string> raport = new List<string>();
            var selItemTag = _itemRepo.GetItemTagByTagId(listItem.selectedItemTag);
            if (listItem.selectedItemId != null)
            {
                foreach (var item in listItem.selectedItemId)
                {
                    if (selItemTag != null)
                    {
                        var checkTag = _itemRepo.GetConnectItemTags(item).Where(i => i.ItemTagId == selItemTag.Id).Count();
                        if (checkTag == 0)
                        {
                            _itemRepo.AddConnectionItemTags(item, selItemTag);
                        }
                        else
                        {
                            raport.Add($"Produkt o Id:{item} posiada już określony tag {selItemTag.Name}");
                        }

                    }
                    if (listItem.selectDeleteItemTags > 0)
                    {
                        _itemRepo.DeleteConnectionItemTagFromItem(item, listItem.selectDeleteItemTags);
                    }
                    if (listItem.selectedNewCategory > 0)
                    {
                        var tempItem = _itemRepo.GetAllItems().FirstOrDefault(c => c.Id == item);
                        if (tempItem != null)
                        {
                            tempItem.CategoryId = listItem.selectedNewCategory;
                            _itemRepo.UpdateItem(tempItem);
                            // raport ok.
                        }
                        else
                        {
                            // raport błędu
                        }
                    }
                }
            }

            return raport;
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
            item.Categories = _itemRepo.GetAllCategories()
                .ProjectTo<CategoryForListVm>(_mapper.ConfigurationProvider).ToList();
            item.ListGroup = _groupItemRepo.GetAllGroupItem()
                .ProjectTo<GroupItemForListVm>(_mapper.ConfigurationProvider).ToList();

            return item;
        }
        public int UpdateItem(AddItemVm model)
        {
            var _item = _mapper.Map<Item>(model);
            _item.CategoryId = model.selectedCategory;
            _item.GroupItemId = model.selectedGroup;
            _itemRepo.UpdateItem(_item);
            if (model.EanCode != null && model.EanCode.Length > 0)
            {
                _item.ImageFolder = model.EanCode;
            }
            string folderName;
            if (model.ImageFolder != null || (model.EanCode != null))
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
        public int AddCategory(AddItemVm item)
        {
            var categoryMap = _mapper.Map<Category>(item.Category);
            var id = _itemRepo.AddCategory(categoryMap);
            return id;
        }

        public UpdateCategoryVm ListCategoryToUpdate(string? searchCategory)
        {
            UpdateCategoryVm updateCategoryVm = new UpdateCategoryVm();
            updateCategoryVm.countCategory = new List<CountCategoryVm>();

            if (searchCategory == null)
            {
                var listCategory = _itemRepo.GetAllCategories().ProjectTo<CategoryForListVm>(_mapper.ConfigurationProvider).Take(20).ToList();
                updateCategoryVm.listCategory = listCategory;
                foreach (var category in updateCategoryVm.listCategory)
                {
                    CountCategoryVm countCategory = new CountCategoryVm();
                    var count = _itemRepo.GetItemsByCategoryId(category.Id).Count();
                    countCategory.countItem = count;
                    countCategory.categoryId = category.Id;

                    updateCategoryVm.countCategory.Add(countCategory);
                }
                return updateCategoryVm;
            }

            updateCategoryVm.listCategory = _itemRepo.GetAllCategories().Where(t => t.Name.Contains(searchCategory))
                .ProjectTo<CategoryForListVm>(_mapper.ConfigurationProvider).ToList();
            return updateCategoryVm;
        }

        public UpdateCategoryVm GetCategoryToUpdate(int categoryId)
        {
            UpdateCategoryVm updateCategory = new UpdateCategoryVm();
            var result = _itemRepo.GetCategoryById(categoryId);
            var categoryMap = _mapper.Map<CategoryForListVm>(result);
            updateCategory.category = categoryMap;
            return updateCategory;
        }

        public void DeleteCategory(int categoryId)
        {
            throw new NotImplementedException();
        }

        public void UpdateCategory(UpdateCategoryVm category, int options)
        {
            if (category != null && options == 1)
            {
                var mapCategory = _mapper.Map<Category>(category.category);
                _itemRepo.UpdateCategory(mapCategory);
            }
            if (category != null && options == 2)
            {
                _itemRepo.DeleteCategory(category.category.Id);
            }
            if (category != null && options == 3)
            {
                var mapCategory = _mapper.Map<Category>(category.category);
                _itemRepo.AddCategory(mapCategory);
            }
        }

        public int AddWarehouse(UpdateWarehouseVm model)
        {
            var mapedModel = _mapper.Map<Warehouse>(model.warehouse);
            var modelId = _itemRepo.AddWarehouse(mapedModel);
            return modelId;
        }

        public UpdateWarehouseVm ListWarehouseToUpdate(string? searchWarehouse)
        {
            UpdateWarehouseVm updateWarehouse = new UpdateWarehouseVm();
            updateWarehouse.countWarehouse = new List<CountWarehouseVm>();
            if (searchWarehouse == null)
            {
                var listWarehouse = _itemRepo.GetAllWarehouses().ProjectTo<WarehouseForListVm>(_mapper.ConfigurationProvider).Take(20).ToList();
                updateWarehouse.listWarehouse = listWarehouse;
                foreach (var warehouse in updateWarehouse.listWarehouse)
                {
                    CountWarehouseVm countWarehouseVm = new CountWarehouseVm();
                    var count = _itemRepo.GetAllItemWarehouses().Where(i => i.WarehouseId == warehouse.Id).Count();
                    countWarehouseVm.countItem = count;
                    countWarehouseVm.warehouseId = warehouse.Id;
                    updateWarehouse.countWarehouse.Add(countWarehouseVm);
                }
                return updateWarehouse;
            }
            updateWarehouse.listWarehouse = _itemRepo.GetAllWarehouses().Where(w => w.Name.Contains(searchWarehouse))
                  .ProjectTo<WarehouseForListVm>(_mapper.ConfigurationProvider).Take(20).ToList();
            return updateWarehouse;
        }

        public UpdateWarehouseVm GetWarehouseToUpdate(int warehouseId)
        {
            UpdateWarehouseVm updateWarehouseVm = new UpdateWarehouseVm();
            var result = _itemRepo.GetAllWarehouses().FirstOrDefault(w => w.Id == warehouseId);
            var mappedResult = _mapper.Map<WarehouseForListVm>(result);
            updateWarehouseVm.warehouse = mappedResult;
            return updateWarehouseVm;
        }

        public void UpdateWarehouse(UpdateWarehouseVm updateWarehouse, int options)
        {
            if (updateWarehouse != null && options == 1)
            {
                var mapWarehouse = _mapper.Map<Warehouse>(updateWarehouse.warehouse);
                _itemRepo.UpdateWarehouse(mapWarehouse);
            }
            if (updateWarehouse != null && options == 2)
            {
                _itemRepo.DeleteWarehouse(updateWarehouse.warehouse.Id);
            }
            if (updateWarehouse != null && options == 3)
            {
                var mapWarehouse = _mapper.Map<Warehouse>(updateWarehouse.warehouse);
                _itemRepo.AddWarehouse(mapWarehouse);
            }
        }

        public ListGroupItemForListVm GroupsItemsList(int options, GroupItemForListVm groupItem)
        {
            ListGroupItemForListVm listGroupItemForListVm= new ListGroupItemForListVm();
            var groupItemList = _groupItemRepo.GetAllGroupItem()
                .ProjectTo<GroupItemForListVm>(_mapper.ConfigurationProvider);
            var mappedGroup = _mapper.Map<GroupItem>(groupItem);
            switch (options)
            {
                case 1:// Dodanie grupy poprzez POST              
                    _groupItemRepo.AddGroupItem(mappedGroup);                   
                    return listGroupItemForListVm;
                case 2: //Update grupy poprzez POST
                    _groupItemRepo.UpdateGroupItem(mappedGroup);
                    return listGroupItemForListVm;
                case 3:// Usunięcie grupy
                    _groupItemRepo.DeleteGroupItem(mappedGroup.Id);
                    return listGroupItemForListVm;
                default: // Pobranie grup do controlera GET
                    listGroupItemForListVm.ListGroupItem = groupItemList.ToList();
                    return listGroupItemForListVm;
            }
        }
    }
}
