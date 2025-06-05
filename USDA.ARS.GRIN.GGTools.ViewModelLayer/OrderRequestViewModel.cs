using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using USDA.ARS.GRIN.GGTools.DataLayer;

namespace USDA.ARS.GRIN.GGTools.ViewModelLayer
{
    public class OrderRequestViewModel : OrderRequestViewModelBase, IViewModel<OrderRequest>
    {
        public void Delete()
        {
            throw new NotImplementedException();
        }

        public void Get(int entityId)
        {
            try
            {
                using (OrderRequestManager mgr = new OrderRequestManager())
                {
                    Entity = mgr.Get(entityId);
                }
            }
            catch (Exception ex)
            {
                PublishException(ex);
                throw (ex);
            }
        }

        public List<OrderRequest> GetAll() {  throw new NotImplementedException(); }

        public void GetItems(int orderRequestId)
        {
            using (OrderRequestManager mgr = new OrderRequestManager())
            { 
               DataCollectionItems = new Collection<OrderRequestItem>(mgr.GetItems(orderRequestId));
            }
        }

        public void GetActions(int orderRequestId)
        {
            using (OrderRequestManager mgr = new OrderRequestManager())
            {
                DataCollectionAction = new Collection<OrderRequestAction>(mgr.GetActions(orderRequestId));
            }
        }

        public void GetAttachments(int orderRequestId)
        {
            using (OrderRequestManager mgr = new OrderRequestManager())
            {
                DataCollectionAttachments = new Collection<OrderRequestAttachment>(mgr.GetAttachments(orderRequestId));
            }
        }

        public void GetPhytoLog(int orderRequestId)
        {
            using (OrderRequestManager mgr = new OrderRequestManager())
            {
                DataCollectionPhytoLog = new Collection<OrderRequestPhytoLog>(mgr.GetPhytoLog(orderRequestId));
            }
        }

        public int Insert()
        {
            throw new NotImplementedException();
        }

        public void Search()
        {
            using (OrderRequestManager mgr = new OrderRequestManager())
            {
                try
                {
                    DataCollection = new Collection<OrderRequest>(mgr.Search(SearchEntity));
                    RowsAffected = mgr.RowsAffected;

                    if (RowsAffected == 1)
                    {
                        Entity = DataCollection[0];
                    }

                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
            }
        }

        public List<OrderRequest> SearchNotes(string searchText)
        {
            throw new NotImplementedException();
        }

        public int Update()
        {
            throw new NotImplementedException();
        }

        OrderRequest IViewModel<OrderRequest>.Get(int entityId)
        {
            throw new NotImplementedException();
        }
    }
}
