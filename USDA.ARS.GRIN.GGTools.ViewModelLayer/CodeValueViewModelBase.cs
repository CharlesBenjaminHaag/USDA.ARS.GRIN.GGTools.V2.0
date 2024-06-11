using System;
using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;

namespace USDA.ARS.GRIN.GGTools.ViewModelLayer
{
    public class CodeValueViewModelBase: AppViewModelBase
    {
        private string _NewGroup;
        private string _SelectedGroup;
        private int _SelectedCodeValueID;
        private CodeValue _Entity = new CodeValue();
        private CodeValueSearch _SearchEntity = new CodeValueSearch();
        private Collection<CodeValue> _DataCollection = new Collection<CodeValue>();
        private Collection<CodeValue> _DataCollectionGroups = new Collection<CodeValue>();

        public CodeValueViewModelBase()
        {
            List<CodeValue> codeValues = new List<CodeValue>();

            using (CodeValueManager  mgr = new CodeValueManager())
            {
                codeValues = mgr.GetGroups();
                Groups = new SelectList(codeValues, "Value", "Title");
                DataCollectionGroups = new Collection<CodeValue>(codeValues);
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

        public string SelectedGroup
        {
            get { return _SelectedGroup; }
            set { _SelectedGroup = value; }
        }

        public int SelectedCodeValueID
        {
            get { return _SelectedCodeValueID; }
            set { _SelectedCodeValueID = value; }
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
        public Collection<CodeValue> DataCollectionGroups
        {
            get { return _DataCollectionGroups; }
            set { _DataCollectionGroups = value; }
        }
    }
}
