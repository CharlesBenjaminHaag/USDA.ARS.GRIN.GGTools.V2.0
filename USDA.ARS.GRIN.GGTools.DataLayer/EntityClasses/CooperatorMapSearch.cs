using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using USDA.ARS.GRIN.GGTools.AppLayer;

namespace USDA.ARS.GRIN.GGTools.DataLayer 
{
    public class CooperatorMapSearch: AppEntityBase
    {
        public int CooperatorID { get; set; }
        public string CooperatorName { get; set; }
        public string GroupTag { get; set; }
    }
}
