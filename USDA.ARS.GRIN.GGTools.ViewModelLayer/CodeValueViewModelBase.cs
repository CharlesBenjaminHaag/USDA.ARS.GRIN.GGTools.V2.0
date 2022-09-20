using System;
using System.Web;
using System.Web.Mvc;
using System.Collections.ObjectModel;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;

namespace USDA.ARS.GRIN.GGTools.ViewModelLayer
{
    public class CodeValueViewModelBase: AppViewModelBase
    {
        private string _NewGroup;
        private CodeValue _Entity = new CodeValue();
        private CodeValueSearch _SearchEntity = new CodeValueSearch();
        private Collection<CodeValue> _DataCollection = new Collection<CodeValue>();

        public CodeValueViewModelBase()
        {
            using (CodeValueManager  mgr = new CodeValueManager())
            {
                Groups = new SelectList(mgr.GetGroups(), "Value", "Title");
                SysLangs = new SelectList(mgr.GetSysLangs(), "Value", "Title");
            }
        }

        #region Select Lists

        public SelectList Groups { get; set; }
        public SelectList SysLangs { get; set; }
        #endregion

        public string NewGroup 
        {
            get { return _NewGroup; }
            set { _NewGroup = value; }
        }

        public CodeValue Entity
        {
            get { return _Entity; }
            set { _Entity = value; }
        }

        public CodeValueSearch SearchEntity
        {
            get { return _SearchEntity; }
            set { _SearchEntity = value; }
        }

        public Collection<CodeValue> DataCollection
        {
            get { return _DataCollection; }
            set { _DataCollection = value; }
        }
    }
}
