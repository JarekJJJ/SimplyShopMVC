using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Application.ViewModels.Article
{
    public class ListArticleForListVm
    {
        public List<ArticleForListVm> Articles { get; set; }
       // public List<string> AllTags { get; set; }
        public int Count { get; set; }


    }
}
