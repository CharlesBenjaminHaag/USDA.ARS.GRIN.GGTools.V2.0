using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web.Mvc;
using USDA.ARS.GRIN.Common.Library.Email;
using USDA.ARS.GRIN.GGTools.DataLayer;

namespace USDA.ARS.GRIN.GGTools.ViewModelLayer
{
    public class CooperatorRecordTransferViewModel
    {
        private int _SourceCooperatorID;
        private int _TargetCooperatorID;
        private string _SourceTableList;
        private string _TargetTableList;
        private Collection<ReportItem> _DataCollectionSourceCooperatorRecords = new Collection<ReportItem>();
        private Collection<ReportItem> _DataCollectionTargetCooperatorRecords = new Collection<ReportItem>();

        public CooperatorRecordTransferViewModel()
        {
            
        }

        public int SourceCooperatorID 
        {
            get { return _SourceCooperatorID; }
            set { _SourceCooperatorID = value; }
        }

        public int TargetCooperatorID
        {
            get { return _TargetCooperatorID; }
            set { _TargetCooperatorID = value; }
        }

        public string SourceTableList
        {
            get { return _SourceTableList; }
            set { _SourceTableList = value; }
        }

        public SelectList SourceCooperators { get; set; }
        public SelectList TargetCooperators { get; set; }

        public Collection<ReportItem> DataCollectionSourceCooperatorRecords
        {
            get { return _DataCollectionSourceCooperatorRecords; }
            set { _DataCollectionSourceCooperatorRecords = value; }
        }
        public Collection<ReportItem> DataCollectionTargetCooperatorRecords
        {
            get { return _DataCollectionTargetCooperatorRecords; }
            set { _DataCollectionTargetCooperatorRecords = value; }
        }
        public void GetSiteCooperators(int siteId)
        {
            using (CooperatorManager mgr = new CooperatorManager())
            {
                SourceCooperators = new SelectList(mgr.Search(new CooperatorSearch { SiteID = siteId }), "ID", "FullName");
                TargetCooperators = new SelectList(mgr.Search(new CooperatorSearch { SiteID = siteId }), "ID", "FullName");
            }
        }
        //public void Transfer()
        //{
        //    //TODO iterate through table list and call transfer for each 
        //    using (CooperatorManager mgr = new CooperatorManager())
        //    {
        //        string[] tableNameCollection ;
        //        tableNameCollection = SourceTableList.Split(',');
        //        foreach (var tableName in tableNameCollection)
        //        {
        //            mgr.Transfer(SourceCooperatorID, TargetCooperatorID, tableName);
        //        }
        //    }
        //    //TODO
        //}
    }
}
