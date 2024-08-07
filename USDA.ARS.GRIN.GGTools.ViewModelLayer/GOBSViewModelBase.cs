using System;
using System.Web.Mvc;
using System.Collections.ObjectModel;
using System.Data;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.GOBS.DataLayer;
using USDA.ARS.GRIN.GRINGlobal.DTO;

namespace USDA.ARS.GRIN.GGTools.GOBS.ViewModelLayer
{
    public class GOBSViewModelBase : AppViewModelBase
    {
        private Dataset _Entity = new Dataset();
        private DatasetField _DatasetField = new DatasetField();
        private DatasetAttach _DatasetAttach = new DatasetAttach();
        private DatasetInventory _DatasetInventory = new DatasetInventory();
        private DatasetMarker _DatasetMarker = new DatasetMarker();
        private DatasetMarkerField _DatasetMarkerField = new DatasetMarkerField();

        private Collection<Dataset> _DataCollectionDatasets = new Collection<Dataset>();
        private Collection<GOBSDataset> _DataCollection = new Collection<GOBSDataset>();
        private DataTable _DataCollectionDataTable = new DataTable();

        public GOBSViewModelBase()
        {
        }
       
        public Dataset Entity
        {
            get { return _Entity; }
            set { _Entity = value; }
        }

        public DatasetAttach DatasetAttach
        {
            get { return _DatasetAttach; }
            set { _DatasetAttach = value; }
        }

        public DatasetField DatasetField
        {
            get { return _DatasetField; }
            set { _DatasetField = value; }
        }

        public DatasetInventory DatasetInventory
        {
            get { return _DatasetInventory; }
            set { _DatasetInventory = value; }
        }

        public DatasetMarker DatasetMarker
        {
            get { return _DatasetMarker; }
            set { _DatasetMarker = value; }
        }
       
        public Collection<Dataset> DataCollectionDatasets
        {
            get { return _DataCollectionDatasets; }
            set { _DataCollectionDatasets = value; }
        }

       //public GOBSMarker Marker
       // {
       //     get { return _Marker; }
       //     set { _Marker = value; }
       // }
    }
}
