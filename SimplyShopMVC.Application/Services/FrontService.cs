using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using SimplyShopMVC.Application.ViewModels.Front;
using SimplyShopMVC.Domain.Interface;
using SimplyShopMVC.Application.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimplyShopMVC.Domain.Model;
using SimplyShopMVC.Application.ViewModels.Item;
using AutoMapper.QueryableExtensions;

namespace SimplyShopMVC.Application.Services
{
    public class FrontService : IFrontService
    {
        private readonly ISupplierRepository _supplierRepo;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHost;
        private readonly IItemRepository _itemRepo;
        private readonly IGroupItemRepository _groupItemRepo;
        private readonly ICategoryTagsRepository _categoryTagsRepo;
        public FrontService(ISupplierRepository supplierRepo, IMapper mapper, IWebHostEnvironment webHost, IItemRepository itemRepo, IGroupItemRepository groupItemRepo, ICategoryTagsRepository categoryTagsRep)
        {
            _supplierRepo = supplierRepo;
            _mapper = mapper;
            _webHost = webHost;
            _itemRepo = itemRepo;
            _groupItemRepo = groupItemRepo;
            _categoryTagsRepo = categoryTagsRep;
        }

        public FrontItemForList GetItemDetail(int id)
        {
            List<FrontItemForList> frontItemList = new List<FrontItemForList>();
            List<Item> itemList = new List<Item>();
           var itemToMap = _itemRepo.GetItemById(id);
            itemList.Add(itemToMap);
            var mappedList = mapItemToList(itemList, frontItemList);
            var frontItem = mappedList.FirstOrDefault();           
            return frontItem; 
        }
        public IndexListVm GetItemsToIndex(int quantityItem) //DO PRZEMYŚLENIA - można dodać zmienne takie jak List<int>tagId, CategoryId, int przedmiotów do pobrania i obsłużyć jedną funkcją wszystkie strony sklepu 
        {
            IndexListVm indexList = new IndexListVm();

            List<FrontItemForList> frontItemForLists = new List<FrontItemForList>();
            List<Item> itemList = new List<Item>();
            int doWhileCount = 0;
            do
            {
                if (quantityItem > 0)
                {
                    doWhileCount = quantityItem;
                    itemList = _itemRepo.GetAllItems().OrderBy(x => Guid.NewGuid()).Take(1).Where(i => i.IsActive == true && i.IsDeleted == false).ToList();
                }
                var mapedList = mapItemToList(itemList, frontItemForLists);
                frontItemForLists.AddRange(mapedList);
            } while (frontItemForLists.Count <= 7);
            indexList.frontItemForLists = frontItemForLists;
            return indexList;
        }
        public IndexListVm GetItemsbyTag(string tag)
        {
            var indexList = new IndexListVm();
            return indexList;
        }
        public ListItemShopIndexVm GetItemsByCategory(int categoryId, int pageSize, int pageNo, string searchItem, int selectedTags)
        {
            var listItem = new ListItemShopIndexVm();
            listItem.tags = new List<ItemTagsForListVm>();
            List<Item> itemList = new List<Item>();
            List<FrontItemForList> frontItemForLists = new List<FrontItemForList>();
           
            if (selectedTags > 0)
            {
                itemList = _itemRepo.GetItemsByTagId(selectedTags).Where(i => i.Name.Contains(searchItem) || i.EanCode.Contains(searchItem) || i.ItemSymbol.Contains(searchItem) && i.CategoryId == categoryId).OrderBy(i => i.Name).ToList();
            }
            else
            {
                itemList = _itemRepo.GetItemsByCategoryId(categoryId).Where(i => i.Name.Contains(searchItem) || i.EanCode.Contains(searchItem) || i.ItemSymbol.Contains(searchItem)).OrderBy(i => i.Name).ToList();
            }
            var mappedItems = mapItemToList(itemList, frontItemForLists);
            var mappedItemsToShow = mappedItems.Skip(pageSize * (pageNo - 1)).Take(pageSize).ToList();
            listItem.count = mappedItems.Count;
            listItem.pageSize = pageSize;
            listItem.currentPage = pageNo;
            listItem.searchItem = searchItem;
            listItem.selectedCategory = categoryId;
            listItem.categoryItems = mappedItemsToShow;
            listItem.selectedTag = selectedTags;
            var categoryTags = _categoryTagsRepo.GetAllCategoryTags().Where(i => i.CategoryId == categoryId).ToList();
            foreach (var tag in categoryTags)
            {
                ItemTagsForListVm tagsForListVm= new ItemTagsForListVm();
                var itemTag = _mapper.Map<ItemTagsForListVm>(_itemRepo.GetAllItemTags().FirstOrDefault(i => i.Id == tag.ItemTagId));              
                listItem.tags.Add(itemTag);
            }
            return listItem;

        }
        public ListItemShopIndexVm GetAllCategories()
        {
            ListItemShopIndexVm listItemShopIndexVm = new ListItemShopIndexVm();
            var listCategories = new List<CategoryForListVm>();
            var receivedCategories = _itemRepo.GetAllCategories()
                .ProjectTo<CategoryForListVm>(_mapper.ConfigurationProvider).ToList();
            listItemShopIndexVm.categories = receivedCategories;
            return listItemShopIndexVm;
        }

        //funkcje 
        public decimal GetPriceDetalB(decimal netPurchasePrice, int vatRateValue, int groupMarkup)
        {
            decimal markupPercentage = (decimal)groupMarkup / 100;
            decimal vatPercentage = (decimal)vatRateValue / 100;

            // Wyliczenie ceny brutto
            decimal priceDetalB = netPurchasePrice * (1 + markupPercentage) * (1 + vatPercentage);

            return priceDetalB;
        }
        public List<FrontItemForList> mapItemToList(List<Item> items, List<FrontItemForList> frontItems)
        {
            IndexListVm indexList = new IndexListVm();

            List<FrontItemForList> frontItemForLists = new List<FrontItemForList>();
            foreach (var item in items)
            {
                var checkDuplicateItem = frontItems.FirstOrDefault(f => f.id == item.Id);
                var indexItemWare = _itemRepo.GetAllItemWarehouses().FirstOrDefault(i => i.ItemId == item.Id);
                if (indexItemWare != null && checkDuplicateItem == null)
                {
                    var vatRateResoult = _itemRepo.GetAllVatRate().FirstOrDefault(v => v.Id == indexItemWare.VatRateId);  //Dodać sprawdzenie czy istnieje przedmiot w itemWArehouse!
                    FrontItemForList indexItem = new FrontItemForList();
                    indexItem.id = item.Id;
                    indexItem.name = item.Name;
                    indexItem.description = item.Description;
                    indexItem.eanCode = item.EanCode;
                    indexItem.imageFolder = item.ImageFolder;
                    indexItem.itemSymbol = item.ItemSymbol;
                    var groupIdresoult = _groupItemRepo.GetAllGroupItem().FirstOrDefault(g => g.Id == item.GroupItemId);
                    var warehouseResoult = _itemRepo.GetAllWarehouses().FirstOrDefault(w => w.Id == indexItemWare.WarehouseId);
                    //Można dodać sprawdzenie czy cena sprzedaży nie została nadana ręcznie w ItemWarehouse jeżeli nie to pobranie marży po grupie.              
                    var resultPriceB = GetPriceDetalB((decimal)indexItemWare.NetPurchasePrice.Value, vatRateResoult.Value, groupIdresoult.PriceMarkupA);
                    indexItem.priceB = resultPriceB;
                    indexItem.quantity = indexItemWare.Quantity;
                    indexItem.deliveryTime = warehouseResoult.DeliveryTime;
                    var _pathImage = $"{_webHost.WebRootPath}\\media\\itemimg\\{item.ImageFolder}\\";
                    var imageToList = ImageHelper.AllImageFromPath(_pathImage).ToList();
                    var listImage = new List<PhotoItemVm>();
                    int photoId = 0;
                    if (imageToList.Count > 0)
                    {
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
                    }
                    else
                    {
                        var photoDetail = new PhotoItemVm();
                        photoDetail.Id = photoId;
                        photoId++;
                        photoDetail.Name = "nophoto.png";
                        var _imgFullUrl = $"/media/{photoDetail.Name}";
                        photoDetail.ImageUrl = _imgFullUrl;
                        photoDetail.IsSelected = false;
                        listImage.Add(photoDetail);
                    }
                    indexItem.tags = ListTagsForItem(item.Id);
                    indexItem.images = listImage;
                    frontItemForLists.Add(indexItem);
                }
            }
            return frontItemForLists;
        }
        public List<ItemTagsForListVm> ListTagsForItem(int itemId) //Dodaje do vm listę tagów do konkretnego itema
        {
            List<ItemTagsForListVm> itemTagsToList = new List<ItemTagsForListVm>();

            var listTag = _itemRepo.GetAllConnectedItemTags().Where(i => i.ItemId == itemId);
            if (listTag.Any())
            {
                foreach (var tags in listTag)
                {
                    ItemTagsForListVm itemTags = new ItemTagsForListVm();
                    var tag = _itemRepo.GetAllItemTags().FirstOrDefault(t => t.Id == tags.ItemTagId);
                    if (tag != null)
                    {
                        itemTags.Id = tag.Id;
                        itemTags.Name = tag.Name;
                        itemTags.Description = tag.Description;
                        itemTagsToList.Add(itemTags);
                    }

                }
            }
            // itemTagsToList.AddRange(listTag);

            return itemTagsToList;
        }
    }
}
