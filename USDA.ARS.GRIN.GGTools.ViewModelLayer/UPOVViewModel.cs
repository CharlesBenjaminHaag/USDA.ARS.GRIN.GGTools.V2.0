using System;
using System.Web.Mvc;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using USDA.ARS.GRIN.Common.Library.Exceptions;
using USDA.ARS.GRIN.Common.Library.Security;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.DataLayer.EntityClasses.UPOV;
using System.Collections.Generic;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer
{
    public class UPOVViewModel : ViewModelBase
    {
        public List<upovCodeItem> DataCollection { get; set; }
    }
}
