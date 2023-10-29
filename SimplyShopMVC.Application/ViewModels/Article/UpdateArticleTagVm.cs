using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Application.ViewModels.Article
{
    public class UpdateArticleTagVm
    {
        public List<ArticleTagsForListVm> TagList { get; set; }
        public ArticleTagsForListVm Tag { get; set; }
        public List<CountArticleVm> Count { get; set; }
        public string searchTag { get; set; }
    }
}
