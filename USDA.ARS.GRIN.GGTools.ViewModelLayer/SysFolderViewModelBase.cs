using System;
using System.Web;
using System.Web.Mvc;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;

namespace USDA.ARS.GRIN.GGTools.ViewModelLayer
{
    public class SysFolderViewModelBase: AuthenticatedViewModelBase
    {
        private SysFolder _Entity = new SysFolder();
        private SysTag _TagEntity = new SysTag();
        private SysFolderSearch _SearchEntity = new SysFolderSearch();
        private Collection<SysFolder> _DataCollection = new Collection<SysFolder>();
        private Collection<SysFolder> _DataCollectionUserFolders = new Collection<SysFolder>();
        private Collection<AppUserItemDynamicFolder> _DataCollectionDynamicFolders = new Collection<AppUserItemDynamicFolder>();
        private Collection<ReportItem> _DataCollectionIDTypes = new Collection<ReportItem>();
        private List<SysFolder> _DataCollectionBatch = new List<SysFolder>();

        public SysFolderViewModelBase()
        {
           
        }

        public SysFolderViewModelBase(int cooperatorId)
        {
            //TableName = "app_user_item_folder";
            //List<CodeValue> categories = new List<CodeValue>();
            //using (SysFolderManager mgr = new SysFolderManager())
            //{
            //    Cooperators = new SelectList(mgr.GetCooperators(TableName), "ID", "FullName");
            //    YesNoOptions = new SelectList(mgr.GetYesNoOptions(), "Key", "Value");

            //    categories = mgr.GetCategories(cooperatorId);

            //    SysFolderSearch searchEntity = new SysFolderSearch();
            //    searchEntity.CreatedByCooperatorID = cooperatorId;
            //    DataCollectionUserFolders = new Collection<SysFolder>(mgr.Search(searchEntity));
            //    Categories = new SelectList(categories, "Value", "Title");
            //}
        }

        public SysFolder Entity
        {
            get { return _Entity; }
            set { _Entity = value; }
        }

        public SysFolderSearch SearchEntity
        {
            get { return _SearchEntity; }
            set { _SearchEntity = value; }
        }

        public SysTag TagEntity
        {
            get { return _TagEntity; }
            set { _TagEntity = value; }
        }

        public Collection<SysFolder> DataCollection
        {
            get { return _DataCollection; }
            set { _DataCollection = value; }
        }
      
    }
}
