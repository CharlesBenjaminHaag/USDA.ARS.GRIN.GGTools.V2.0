using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.GOBS.DataLayer;
using System;
using System.Web.Mvc;
using System.Collections.ObjectModel;
using System.Data;

namespace USDA.ARS.GRIN.GGTools.GOBS.ViewModelLayer
{
    public class GOBSViewModelBase : AppViewModelBase
    {
        private GOBSDataset _Entity = new GOBSDataset();
        
        private GOBSDatasetSearch _SearchEntity = new GOBSDatasetSearch();
        private Collection<GOBSDataset> _DataCollection = new Collection<GOBSDataset>();
        private DataTable _DataCollectionDataTable = new DataTable();

        public GOBSViewModelBase()
        {
            //using (GOBSManager mgr = new GOBSManager())
            //{
            //    Cooperators = new SelectList(mgr.GetCooperators("taxonomy_GOBS"), "ID", "FullName");
            //}
        }
       
        public GOBSDataset Entity
        {
            get { return _Entity; }
            set { _Entity = value; }
        }

        public GOBSDatasetSearch SearchEntity
        {
            get { return _SearchEntity; }
            set { _SearchEntity = value; }
        }

        public Collection<GOBSDataset> DataCollection
        {
            get { return _DataCollection; }
            set { _DataCollection = value; }
        }

        public DataTable DataCollectionDataTable
        {
            get { return _DataCollectionDataTable; }
            set { _DataCollectionDataTable = value; }
        }
    }
}
