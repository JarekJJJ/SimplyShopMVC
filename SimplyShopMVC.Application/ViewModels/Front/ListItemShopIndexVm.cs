using SimplyShopMVC.Application.ViewModels.Item;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Application.ViewModels.Front
{
    public class ListItemShopIndexVm
    {
        public List<FrontItemForList> newsItems { get; set; }
        public List<FrontItemForList> recomendedItems { get; set; }
        public List<FrontItemForList> promoItems { get; set; }
        public List<FrontItemForList> categoryItems { get; set; }
        public List<FrontItemForList> tagItems { get; set; }
        public List<CategoryForListVm> categories { get; set; }
        public List<ItemTagsForListVm> tags { get; set; }
        public int selectedCategory { get; set; }
        public int selectedTag { get; set; }
        public int selectedView { get; set; }
        public int count { get; set; }
        public int currentPage { get; set; }
        public int pageSize { get; set; }
        public string searchItem { get; set; }
    }
}
