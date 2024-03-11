using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using SimplyShopMVC.Application.Helpers;
using SimplyShopMVC.Application.Interfaces;
using SimplyShopMVC.Application.ViewModels.Article;
using SimplyShopMVC.Application.ViewModels.Front;
using SimplyShopMVC.Application.ViewModels.Item;
using SimplyShopMVC.Application.ViewModels.PcSets;
using SimplyShopMVC.Domain.Interface;
using SimplyShopMVC.Domain.Model;
using SimplyShopMVC.Domain.Model.Sets;
using SimplyShopMVC.Infrastructure.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Application.Services
{
    public class SetService : ISetService
    {
        private readonly IMapper _mapper;
        private readonly IItemRepository _itemRepo;
        private readonly IGroupItemRepository _groupItemRepo;
        private readonly ISetsRepository _setsRepo;
        private readonly IPriceCalculate _priceCalc;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IUserRepository _userRepo;
        public SetService(IMapper mapper, IItemRepository itemRepo, IGroupItemRepository groupItemRepo, ISetsRepository setsRepo, IPriceCalculate priceCalc, IWebHostEnvironment webHostEnvironment, IUserRepository userRepository)
        {
            _mapper = mapper;
            _itemRepo = itemRepo;
            _groupItemRepo = groupItemRepo;
            _setsRepo = setsRepo;
            _priceCalc = priceCalc;
            _webHostEnvironment = webHostEnvironment;
            _userRepo = userRepository;
        }

        public ListPcSetsForListVm SetHandling(ListItemShopIndexVm result, int options)
        {
            string _catalog = "";
            if (result.pcSets != null)
            {
                _catalog = result.pcSets.Id.ToString();
            }
            ListPcSetsForListVm newListPcSets = new ListPcSetsForListVm();
            newListPcSets.pcSet = new PcSetsForListVm();
            var basePcSets = _mapper.Map<PcSetsForListVm>(_setsRepo.GetAllPcSets().FirstOrDefault(s => s.IsSaved == false));
            if (basePcSets == null && options <= 1)
            {
                newListPcSets.pcSet = new PcSetsForListVm();
                newListPcSets.setsItems = new List<SetsItemForListVm>();
            }
            if (basePcSets != null && options <= 1)
            {
                newListPcSets.pcSet = basePcSets;
                _catalog = basePcSets.Id.ToString();
            }
            if (result.pcSets != null && options == 0)
            {
                _catalog = result.pcSets.Id.ToString();

                options = 6;
            }
            string _pathImage = "";
            if ((result.pcSets != null && result.pcSets.Id > 0) || (newListPcSets.pcSet != null && newListPcSets.pcSet.Id > 0))
            {
                _pathImage = $"{_webHostEnvironment.WebRootPath}\\media\\pcsetimg\\{_catalog}\\";
            }

            switch (options)
            {
                case 1://Dodawanie do zestawu itemu
                    if (basePcSets == null)
                    {
                        newListPcSets.pcSet.GroupItemId = _groupItemRepo.GetAllGroupItem().FirstOrDefault().Id;
                        newListPcSets.pcSet.Title = "Nowy Zestaw";
                        newListPcSets.pcSet.Description = "opis";
                        newListPcSets.pcSet.CreatedDate = DateTime.Now;
                        newListPcSets.pcSet.ShortDescription = "krótki opis";
                        newListPcSets.pcSet.IsActive = false;
                        newListPcSets.pcSet.IsSaved = false;
                        newListPcSets.pcSet.IsDeleted = false;
                        var pcsetId = _setsRepo.AddPcSets(_mapper.Map<PcSets>(newListPcSets.pcSet));
                        basePcSets = new PcSetsForListVm();
                        basePcSets.Id = pcsetId;
                    }
                    var itemToSet = _itemRepo.GetAllItems().FirstOrDefault(i => i.Id == result.listItemsSets.setsItem.ItemId);
                    if (itemToSet != null)
                    {
                        var itemWareToSet = _itemRepo.GetAllItemWarehouses().Where(w => w.ItemId == itemToSet.Id && w.Quantity > 0).OrderBy(w => w.NetPurchasePrice).FirstOrDefault();
                        if (itemWareToSet != null)
                        {
                            SetsItemForListVm setsItem = new SetsItemForListVm();
                            setsItem.ItemId = itemToSet.Id;
                            setsItem.Quantity = result.listItemsSets.setsItem.Quantity;
                            setsItem.VatRateId = itemWareToSet.VatRateId;
                            setsItem.PcSetsId = basePcSets.Id;
                            setsItem.Name = itemToSet.Name;
                            setsItem.NetPurchasePrice = (decimal)itemWareToSet.NetPurchasePrice;
                            setsItem.EanCode = itemToSet.EanCode;
                            setsItem.WarehouseId = itemWareToSet.WarehouseId;
                            _setsRepo.AddPcSetsItem(_mapper.Map<PcSetsItems>(setsItem));
                        }
                    }
                    break;
                case 2: //usuwanie z zestawu itemu
                    _setsRepo.DeletePcSetsItem(result.setItem.Id);
                    break;
                case 3: // edycja i zapisywanie zestawu
                    var selectedSet = _setsRepo.GetAllPcSets().FirstOrDefault(s => s.Id == result.pcSets.Id);
                    var setsItemsForSet = _setsRepo.GetAllPcSetsItems().Where(s => s.PcSetsId == result.pcSets.Id)
                       .ProjectTo<SetsItemForListVm>(_mapper.ConfigurationProvider).ToList();
                    if (setsItemsForSet.Any())
                    {
                        newListPcSets.setsItems = new List<SetsItemForListVm>();
                        newListPcSets.setsItems.AddRange(setsItemsForSet);
                    }
                    selectedSet.GroupItemId = result.pcSets.GroupItemId;
                    selectedSet.Title = result.pcSets.Title;
                    selectedSet.Description = result.pcSets.Description;
                    selectedSet.ShortDescription = result.pcSets.ShortDescription;
                    selectedSet.IsActive = result.pcSets.IsActive;
                    selectedSet.IsSaved = result.pcSets.IsSaved;
                    selectedSet.IsDeleted = result.pcSets.IsDeleted;
                    selectedSet.UpdatedDate = DateTime.Now;
                    _setsRepo.UpdatePcSets(selectedSet);
                    var pcSetoToSave = result.pcSets;
                    //_setsRepo.UpdatePcSets(_mapper.Map<PcSets>(pcSetoToSave));
                    //Dodawanie zdjęć 
                    string folderName;
                    folderName = result.pcSets.Id.ToString();
                    string newFolderPath = Path.Combine(_webHostEnvironment.WebRootPath, "media\\pcsetimg", folderName);
                    try
                    {
                        Directory.CreateDirectory(newFolderPath);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    if (result.Image != null)
                    {
                        foreach (var image in result.Image)
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
                    //var selectedItemSet = _setsRepo.GetAllPcSetsItems().Where(s => s.PcSetsId == result.pcSets.Id)
                    //    .ProjectTo<SetsItemForListVm>(_mapper.ConfigurationProvider).ToList();
                    newListPcSets.pcSet = pcSetoToSave;

                    break;
                case 4:
                    newListPcSets.pcSet.Id = result.setItem.PcSetsId;
                    _setsRepo.UpdatePcSetsItem(_mapper.Map<PcSetsItems>(result.setItem));

                    break;
                case 5:
                    // string newFolderPath = Path.Combine(_webHostEnvironment.WebRootPath, "media\\pcimg", folderName);
                    ImageHelper.DeleteImage(result.selectedImage.Name, _pathImage);
                    newListPcSets.pcSet = result.pcSets;
                    break;
                case 6: //wczytywanie zestawu do edycji
                    selectedSet = _setsRepo.GetAllPcSets().FirstOrDefault(s => s.Id == result.pcSets.Id);
                    setsItemsForSet = _setsRepo.GetAllPcSetsItems().Where(s => s.PcSetsId == result.pcSets.Id)
                       .ProjectTo<SetsItemForListVm>(_mapper.ConfigurationProvider).ToList();
                    if (setsItemsForSet.Any())
                    {
                        newListPcSets.setsItems = new List<SetsItemForListVm>();
                        newListPcSets.setsItems.AddRange(setsItemsForSet);
                    }
                    newListPcSets.pcSet = _mapper.Map<PcSetsForListVm>(selectedSet);
                    _pathImage = $"{_webHostEnvironment.WebRootPath}\\media\\pcsetimg\\{_catalog}\\";
                    break;
                default:
                    newListPcSets.listSets = _setsRepo.GetAllPcSets().ProjectTo<PcSetsForListVm>(_mapper.ConfigurationProvider).ToList();
                    break;
            }
            // var _pathImage = $"{_webHostEnvironment.WebRootPath}\\media\\pcsetimg\\{newListPcSets.pcSet.Id.ToString()}\\";
            var listImage = new List<PhotoItemVm>();
            if (newListPcSets.pcSet.Id != 0)
            {
                var imageToList = ImageHelper.AllImageFromPath(_pathImage).ToList();
                int photoId = 0;
                foreach (var imageUrl in imageToList)
                {
                    var photoDetail = new PhotoItemVm();
                    photoDetail.Id = photoId;
                    photoId++;
                    photoDetail.Name = imageUrl;
                    var _imgFullUrl = $"/media/pcsetimg/{newListPcSets.pcSet.Id.ToString()}/{imageUrl}";
                    photoDetail.ImageUrl = _imgFullUrl;
                    photoDetail.IsSelected = false;
                    listImage.Add(photoDetail);
                }
            }
            newListPcSets.listGroupItem = _groupItemRepo.GetAllGroupItem().ProjectTo<GroupItemForListVm>(_mapper.ConfigurationProvider).ToList();
            newListPcSets.listImages = listImage;

            return newListPcSets;
        }
        public ListPcSetsForListVm ListSetForUser(string userId)
        {
            ListPcSetsForListVm listPcSetsForUser = new ListPcSetsForListVm();
            listPcSetsForUser.listSets = new List<PcSetsForListVm>();
            listPcSetsForUser.setsItems = new List<SetsItemForListVm>();
            var listPcset = _setsRepo.GetAllPcSets().Where(i => i.IsActive == true && i.IsSaved == true && i.IsDeleted == false)
                .ProjectTo<PcSetsForListVm>(_mapper.ConfigurationProvider).ToList();
            if (listPcset != null)
            {
                foreach (var set in listPcset)
                {
                    // PcSetsForListVm setsVm = new PcSetsForListVm();
                    decimal TotalCost = 0;
                    var listItemPcSEt = _setsRepo.GetAllPcSetsItems().Where(i => i.PcSetsId == set.Id)
                        .ProjectTo<SetsItemForListVm>(_mapper.ConfigurationProvider).ToList();
                    if (listItemPcSEt != null)
                    {
                        foreach (var item in listItemPcSEt)
                        {
                            var itemCost = _priceCalc.priceCalc(item.ItemId, set.GroupItemId, item.WarehouseId, userId);
                            itemCost = itemCost * item.Quantity;
                            TotalCost += itemCost;
                            listPcSetsForUser.setsItems.Add(item);
                        }
                        var _pathImage = $"{_webHostEnvironment.WebRootPath}\\media\\pcsetimg\\{set.Id}\\";
                        var imageToList = ImageHelper.AllImageFromPath(_pathImage).ToList().FirstOrDefault();
                        if (!String.IsNullOrEmpty(imageToList))
                        {
                            set.mainImage = $"\\media\\pcsetimg\\{set.Id}\\{imageToList}";
                        }
                        else
                        {
                            set.mainImage = "\\media\\nophoto.png";
                        }
                        set.TotalCost = TotalCost;
                    }
                    listPcSetsForUser.listSets.Add(set);
                }
            }

            return listPcSetsForUser;
        }
        public PcSetDetailVm PcSetDetailForUser(int pcSetId, string userId) // zastanowić się czy nie zrobić nowego vm żeby zdjęcia poszególnych itemów wyśwsietlać
        {
            PcSetDetailVm pcSetVM = new PcSetDetailVm();
            pcSetVM.listSetItem = new List<SetsItemForListVm>();
            pcSetVM.pcSet = new PcSetsForListVm();
            pcSetVM.listItem = new List<ItemForListVm>();
            var listPcset = _mapper.Map<PcSetsForListVm>(_setsRepo.GetAllPcSets().FirstOrDefault(i => i.Id == pcSetId));
            if (listPcset != null)
            {           
                decimal TotalCost = 0;
                var listItemPcSEt = _setsRepo.GetAllPcSetsItems().Where(i => i.PcSetsId == listPcset.Id)
                    .ProjectTo<SetsItemForListVm>(_mapper.ConfigurationProvider).ToList();
                if (listItemPcSEt != null)
                {
                    foreach (var item in listItemPcSEt)
                    {
                        var itemCost = _priceCalc.priceCalc(item.ItemId, listPcset.GroupItemId, item.WarehouseId, userId);
                        itemCost = itemCost * item.Quantity;
                        TotalCost += itemCost;
                        pcSetVM.listSetItem.Add(item);
                        pcSetVM.listItem.Add(_mapper.Map<ItemForListVm>(_itemRepo.GetAllItems().FirstOrDefault(i => i.Id == item.ItemId)));
                    }
                    var _pathImage = $"{_webHostEnvironment.WebRootPath}\\media\\pcsetimg\\{listPcset.Id}\\";
                    var imageToList = ImageHelper.AllImageFromPath(_pathImage).ToList().FirstOrDefault();
                    if (!String.IsNullOrEmpty(imageToList))
                    {
                        listPcset.mainImage = $"\\media\\pcsetimg\\{listPcset.Id}\\{imageToList}";
                    }
                    else
                    {
                        listPcset.mainImage = "\\media\\nophoto.png";
                    }
                    listPcset.TotalCost = TotalCost;
                    pcSetVM.pcSet = listPcset;
                }
            }

            return pcSetVM;
        }

    }
}
