using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Application.ViewModels.Item
{
    public class UpdateCategoryVm
    {
        public List<CategoryForListVm> listCategory { get; set; }
        public CategoryForListVm category { get; set; }
        public List<CountCategoryVm> countCategory { get; set; }
        public string searchCategory { get; set; }
    }
}
