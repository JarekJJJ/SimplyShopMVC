using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Domain.Model
{
    public class ConnectArticleTag
    {
        public int ArticleId { get; set; }
        public Article Article { get; set; }
        public int ArticleTagId { get; set; }
        public ArticleTag ArticleTag { get; set; }
    }
}
