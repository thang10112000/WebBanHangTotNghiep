using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel
{
    public class ProductViewModel
    {
        public long ID { set; get; }
        public string Name { set; get; }
        public string MetaTitle { set; get; }
        public string Decriptions { get; set; }
        public string Images { set; get; }
        public string MoreImages { get; set; }
        public string Link { set; get; }
        public decimal? Price { set; get; }
        public decimal? PromotionPrice { get; set; }
        public long? CategoryID { get; set; }
        public string CateName { set; get; }
        public string CateMetaTitle { set; get; }
        public int? ViewCount { get; set; }
        public bool Status { get; set; }
        public DateTime? CreatedDate { set; get; }
    }
}