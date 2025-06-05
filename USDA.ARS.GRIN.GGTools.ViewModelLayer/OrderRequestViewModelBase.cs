using System;
using System.Web;
using System.Web.Mvc;
using System.Collections.ObjectModel;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;
using System.Linq;

namespace USDA.ARS.GRIN.GGTools.ViewModelLayer
{
    public class OrderRequestViewModelBase : AppViewModelBase
    {
        private OrderRequest _Entity = new OrderRequest();
        private OrderRequestSearch _SearchEntity = new OrderRequestSearch();
        private Collection<OrderRequest> _DataCollection = new Collection<OrderRequest>();
        private Collection<OrderRequestItem> _DataCollectionItems = new Collection<OrderRequestItem>();
        private Collection<OrderRequestAction> _DataCollectionActions = new Collection<OrderRequestAction>();
        private Collection<OrderRequestAttachment> _DataCollectionAttachments = new Collection<OrderRequestAttachment>();
        private Collection<OrderRequestPhytoLog> _DataCollectionPhytoLog = new Collection<OrderRequestPhytoLog>();

        public OrderRequestViewModelBase()
        {
            using (OrderRequestManager mgr = new OrderRequestManager())
            {
                OrderRequestTypeCodes = new SelectList(mgr.GetCodeValues("ORDER_REQUEST_TYPE"), "Value","Title");
                OrderRequestActionCodes = new SelectList(mgr.GetCodeValues("ORDER_REQUEST_ACTION"), "Value", "Title");
                IntendedUseCodes = new SelectList(mgr.GetCodeValues("ORDER_INTENDED_USE"), "Value", "Title");
                Cooperators = new SelectList(mgr.GetCooperators("order_request"), "ID", "FullName");
                TimeFrameOptions = new SelectList(mgr.GetTimeFrameOptions(), "Value", "Title");
                YesNoOptions = new SelectList(mgr.GetYesNoOptions(), "Key", "Value");
            }
        }

        public OrderRequest Entity
        {
            get { return _Entity; }
            set { _Entity = value; }
        }
        public OrderRequestSearch SearchEntity
        {
            get { return _SearchEntity; }
            set { _SearchEntity = value; }
        }

        public Collection<OrderRequest> DataCollection
        { 
            get { return _DataCollection; } 
            set { _DataCollection = value; } 
        }

        public Collection<OrderRequestItem> DataCollectionItems
        {
            get { return _DataCollectionItems; }
            set { _DataCollectionItems = value; }
        }

        public Collection<OrderRequestAction> DataCollectionAction
        {
            get { return _DataCollectionActions; }
            set { _DataCollectionActions = value; }
        }

        public Collection<OrderRequestAttachment> DataCollectionAttachments
        {
            get { return _DataCollectionAttachments; }
            set { _DataCollectionAttachments = value; }
        }

        public Collection<OrderRequestPhytoLog> DataCollectionPhytoLog
        {
            get { return _DataCollectionPhytoLog; }
            set { _DataCollectionPhytoLog = value; }
        }

        #region Select Lists

        public SelectList OrderRequestTypeCodes { get; set; }
        public SelectList OrderRequestActionCodes { get; set; }
        public SelectList IntendedUseCodes { get; set; }
        #endregion
    }
}
