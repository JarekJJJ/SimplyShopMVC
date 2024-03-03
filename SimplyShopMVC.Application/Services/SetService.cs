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
        public SetService(IMapper mapper, IItemRepository itemRepo, IGroupItemRepository groupItemRepo, ISetsRepository setsRepo, IPriceCalculate priceCalc, IWebHostEnvironment webHostEnvironment)
        {
            _mapper = mapper;
            _itemRepo = itemRepo;
            _groupItemRepo = groupItemRepo;
            _setsRepo = setsRepo;
            _priceCalc = priceCalc;
            _webHostEnvironment = webHostEnvironment;
        }

        public ListPcSetsForListVm SetHandling(ListItemShopIndexVm result, int options)
        {
            ListPcSetsForListVm newListPcSets = new ListPcSetsForListVm();
            var basePcSets = _mapper.Map<PcSetsForListVm>(_setsRepo.GetAllPcSets().FirstOrDefault(s => s.IsSaved == false));
            if (basePcSets != null)
            {
                var setsItemsForSet = _setsRepo.GetAllPcSetsItems().Where(s => s.PcSetsId == basePcSets.Id)
                    .ProjectTo<SetsItemForListVm>(_mapper.ConfigurationProvider).ToList();
                newListPcSets.pcSet = basePcSets;
                if (setsItemsForSet.Any())
                {
                    newListPcSets.setsItems = new List<SetsItemForListVm>();
                    newListPcSets.setsItems.AddRange(setsItemsForSet);
                }
            }
            else
            {
                newListPcSets.pcSet = new PcSetsForListVm();
                newListPcSets.setsItems = new List<SetsItemForListVm>();
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
                        _setsRepo.AddPcSets(_mapper.Map<PcSets>(newListPcSets.pcSet));
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
                    break;
                case 3: // edycja i zapisywanie zestawu
                    var selectedSet = _setsRepo.GetAllPcSets().FirstOrDefault(s => s.Id == result.pcSets.Id);
                    selectedSet.Title = result.pcSets.Title;
                    selectedSet.Description = result.pcSets.Description;
                    selectedSet.ShortDescription= result.pcSets.ShortDescription;
                    selectedSet.IsActive= result.pcSets.IsActive;
                    selectedSet.IsSaved= result.pcSets.IsSaved;
                    selectedSet.IsDeleted= result.pcSets.IsDeleted;
                    selectedSet.UpdatedDate= DateTime.Now;
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
                    _setsRepo.UpdatePcSetsItem(_mapper.Map<PcSetsItems>(result.setItem));                  
                    break;
                default:
                    newListPcSets.listSets = _setsRepo.GetAllPcSets().ProjectTo<PcSetsForListVm>(_mapper.ConfigurationProvider).ToList();
                    break;
            }
            var _pathImage = $"{_webHostEnvironment.WebRootPath}\\media\\pcsetimg\\{newListPcSets.pcSet.Id.ToString()}\\";
            var imageToList = ImageHelper.AllImageFromPath(_pathImage).ToList();
            var listImage = new List<PhotoItemVm>();
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
            newListPcSets.listImages= listImage;

            return newListPcSets;
        }

    }
}
