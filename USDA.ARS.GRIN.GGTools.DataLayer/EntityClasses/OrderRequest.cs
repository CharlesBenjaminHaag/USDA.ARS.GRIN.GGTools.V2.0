using System;
using System.Text;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;


namespace USDA.ARS.GRIN.GGTools.DataLayer
{
    public class OrderRequest : AppEntityBase
    {
        public int OriginalID { get; set; }
        public int WebOrderRequestID { get; set; }
        public string OrderTypeCode { get; set; }
        public string OrderTypeDescription { get; set; }
        public DateTime OrderDate { get; set; }
        public string IntendedUseCode { get; set; }
        public string IntendedUseDescription { get; set; }

    }
}
