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
        private GOBSDatasetMarker _DatasetMarker = new GOBSDatasetMarker();
        private GOBSDatasetAttachment _DatasetAttachment = new GOBSDatasetAttachment();
        private GOBSDatasetField _DatasetField = new GOBSDatasetField();
        private GOBSDatasetInventory _DatasetInventory = new GOBSDatasetInventory();
        private GOBSMarker _Marker = new GOBSMarker();
        // ds marker field
        // ds marker value
        // report trait
        // report value
        // type
        private GOBSType _Type = new GOBSType();

        private GOBSDatasetSearch _SearchEntity = new GOBSDatasetSearch();
        private Collection<SysTable> _DataCollectionSysTables = new Collection<SysTable>();
        private Collection<GOBSDataset> _DataCollection = new Collection<GOBSDataset>();
        private DataTable _DataCollectionDataTable = new DataTable();

        public GOBSViewModelBase()
        {
            //{
            //    Cooperators = new SelectList(mgr.GetCooperators("taxonomy_GOBS"), "ID", "FullName");
            //}
        }
       
        public GOBSDataset Entity
        {
            get { return _Entity; }
            set { _Entity = value; }
        }

        public GOBSDatasetAttachment DatasetAttachment
        {
            get { return _DatasetAttachment; }
            set { _DatasetAttachment = value; }
        }

        public GOBSDatasetField DatasetField
        {
            get { return _DatasetField; }
            set { _DatasetField = value; }
        }

        public GOBSDatasetInventory DatasetInventory
        {
            get { return _DatasetInventory; }
            set { _DatasetInventory = value; }
        }

        public GOBSDatasetMarker DatasetMarker
        {
            get { return _DatasetMarker; }
            set { _DatasetMarker = value; }
        }

       //ds m FIELD
       // ds m value
       // ds value
       
       public GOBSMarker Marker
        {
            get { return _Marker; }
            set { _Marker = value; }
        }

        // m field
        // m value
        // r trait
        // r value

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

        public Collection<SysTable> DataCollectionSysTables
        {
            get { return _DataCollectionSysTables; }
            set { _DataCollectionSysTables = value; }
        }

        public DataTable DataCollectionDataTable
        {
            get { return _DataCollectionDataTable; }
            set { _DataCollectionDataTable = value; }
        }
    }
}
