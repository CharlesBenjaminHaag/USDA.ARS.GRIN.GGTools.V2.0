using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer;
using System;
using System.Web;
using System.Web.Mvc;
using System.Collections.ObjectModel;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer
{
   public class CommonNameLanguageViewModelBase : AppViewModelBase
    {
        private CommonNameLanguage _Entity = new CommonNameLanguage();
        private CommonNameLanguageSearch _SearchEntity = new CommonNameLanguageSearch();
        private Collection<CommonNameLanguage> _DataCollection = new Collection<CommonNameLanguage>();

        public CommonNameLanguageViewModelBase()
        {
            TableName = "taxonomy_common_name_language";
            using (CommonNameManager mgr = new CommonNameManager())
            {
                Cooperators = new SelectList(mgr.GetCooperators(TableName), "ID", "FullName");
                Countries = new SelectList(mgr.GetCodeValues("GEOGRAPHY_COUNTRY_CODE"),"Value","Title");
            }
            
        }

        public CommonNameLanguage Entity
        {
            get { return _Entity; }
            set { _Entity = value; }
        }
        public CommonNameLanguageSearch SearchEntity
        {
            get { return _SearchEntity; }
            set { _SearchEntity = value; }
        }
        public Collection<CommonNameLanguage> DataCollection
        {
            get { return _DataCollection; }
            set { _DataCollection = value; }
        }
        public SelectList Countries { get; set; }
    }
}
