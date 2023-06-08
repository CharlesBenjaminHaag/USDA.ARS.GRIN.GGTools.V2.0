using System;
using System.Web.Mvc;
using System.Collections.ObjectModel;
using USDA.ARS.GRIN.Common.Library.Exceptions;
using USDA.ARS.GRIN.Common.Library.Security;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using System.Collections.Generic;

namespace USDA.ARS.GRIN.GGTools.ViewModelLayer
{
    public class SysDynamicQueryViewModel: SysDynamicQueryViewModelBase
    {
        public void Search()
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            using (SysDynamicQueryManager mgr = new SysDynamicQueryManager())
            {
                DataCollectionDataTable = mgr.Search(SearchEntity);
            }
        }
    }
}
