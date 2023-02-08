using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using USDA.ARS.GRIN.GGTools.AppLayer;

namespace USDA.ARS.GRIN.GGTools.DataLayer
{
    public partial class SysTable : AppEntityBase
    {
        public string DatabaseAreaCode { get; set; }
        public string TableNameName { get; set; }
        public string TableTitle { get; set; }
        public string TableCode { get; set; }
        public string IsCited { get; set; }
        public string IsWebPreviewable { get; set; }
    }
}
