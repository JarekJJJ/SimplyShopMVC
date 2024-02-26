using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Domain.Model.Sets
{
    public class PcSets
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int GroupItemId { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public DateTime CreatedDate { get; set; } 
        public DateTime UpdatedDate { get; set; } = DateTime.Now;
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public virtual ICollection<PcSetsItems> PcSetsItems { get; set; }
        public virtual GroupItem GroupItem { get; set; }
    }
}
