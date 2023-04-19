using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USDA.ARS.GRIN.GGTools.AppLayer
{
    public class SearchEntityBase
    {
        public string Environment { get; set; }
        public int FolderID { get; set; }
        public int? ID { get; set; }
        public string IDList { get; set; }
        public string TableName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int CreatedByCooperatorID { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int ModifiedByCooperatorID { get; set; }
        public DateTime? OwnedDate { get; set; }
        public int OwnedByCooperatorID { get; set; }
        public int OwnedByCooperatorSiteID { get; set; }
        public string DateRangeFilter { get; set; }
        public string Note { get; set; }
        public string SQLStatement { get; set; }
        public string SQLWhere { get; set; }
        public string SaveSearchTitle
        { get; set; }
        
        public string SaveSearchDescription
        { get; set; }
        
    }
}
