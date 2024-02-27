using AutoMapper;
using AutoMapper.QueryableExtensions;
using SimplyShopMVC.Application.Interfaces;
using SimplyShopMVC.Application.ViewModels.PcSets;
using SimplyShopMVC.Domain.Interface;
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
        public SetService(IMapper mapper, IItemRepository itemRepo, IGroupItemRepository groupItemRepo, ISetsRepository setsRepo)
        {
            _mapper = mapper;
            _itemRepo = itemRepo;
            _groupItemRepo = groupItemRepo;
            _setsRepo = setsRepo;
        }

        public ListPcSetsForListVm SetHandling(ListPcSetsForListVm listPcSets, int options)
        {
            ListPcSetsForListVm newListPcSets = new ListPcSetsForListVm();
            switch (options)
            {
                case 1:
                    break;
                default:
                    break;
            }
            var basePcSets = _mapper.Map<PcSetsForListVm>(_setsRepo.GetAllPcSets().FirstOrDefault(s=>s.IsSaved==false));
            if(basePcSets != null)
            {
                var setsItemsForSet = _setsRepo.GetAllPcSetsItems().Where(s => s.PcSetsId == basePcSets.Id)
                    .ProjectTo<SetsItemForListVm>(_mapper.ConfigurationProvider).ToList();
                newListPcSets.pcSet = basePcSets;
                newListPcSets.setsItems.AddRange(setsItemsForSet);
            }
            else
            {
                newListPcSets.pcSet = new PcSetsForListVm();
                newListPcSets.setsItems = new List<SetsItemForListVm>();
            }
            
            return newListPcSets;
        }
    }
}
