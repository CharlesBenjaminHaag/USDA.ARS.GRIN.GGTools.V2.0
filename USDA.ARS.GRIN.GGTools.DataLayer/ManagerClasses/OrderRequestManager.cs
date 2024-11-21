using System;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using USDA.ARS.GRIN.Common.DataLayer;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;

namespace USDA.ARS.GRIN.GGTools.OrderManagement.DataLayer.ManagerClasses
{
    public class OrderRequestManager : GRINGlobalDataManagerBase, IManager<OrderRequest, OrderRequestSearch>
    {
        public void BuildInsertUpdateParameters()
        {
            throw new NotImplementedException();
        }

        public int Delete(WebOrderRequest entity)
        {
            throw new NotImplementedException();
        }

        public int Delete(OrderRequest entity)
        {
            throw new NotImplementedException();
        }

        public OrderRequest Get(int entityId)
        {
            SQL = "usp_GRINGlobal_Order_Request_Select";
            OrderRequest orderRequest = new OrderRequest();

            var parameters = new List<IDbDataParameter> {
            CreateParameter("order_request_id", (object)entityId, false)
            };

            orderRequest = GetRecord<OrderRequest>(SQL, CommandType.StoredProcedure, parameters.ToArray());
            return orderRequest;
        }

        public List<OrderRequestItem> GetItems(int orderRequestId)
        {
            List<OrderRequestItem> results = new List<OrderRequestItem>();

            SQL = " SELECT * FROM vw_GRINGlobal_Order_Request_Item ";
            SQL += " WHERE        (@OrderRequestID         IS NULL OR  OrderRequestID       = @OrderRequestID)";

            var parameters = new List<IDbDataParameter> {
                CreateParameter("OrderRequestID", orderRequestId > 0 ? (object)orderRequestId : DBNull.Value, true),
            };

            results = GetRecords<OrderRequestItem>(SQL, parameters.ToArray());
            RowsAffected = results.Count;

            return results;
        }
        
        public List<OrderRequestAction> GetActions(int orderRequestId)
        {
            List<OrderRequestAction> results = new List<OrderRequestAction>();

            SQL = " SELECT * FROM vw_GRINGlobal_Order_Request_Action ";
            SQL += " WHERE        (@OrderRequestID         IS NULL OR  OrderRequestID       = @OrderRequestID)";

            var parameters = new List<IDbDataParameter> {
                CreateParameter("OrderRequestID", orderRequestId > 0 ? (object)orderRequestId : DBNull.Value, true),
            };

            results = GetRecords<OrderRequestAction>(SQL, parameters.ToArray());
            RowsAffected = results.Count;

            return results;
        }

        public List<OrderRequestAttachment> GetAttachments(int entityId)
        {
            List<OrderRequestAttachment> orderRequestAttachments = new List<OrderRequestAttachment>();
            //TODO
            return orderRequestAttachments;
        }

        public List<OrderRequestPhytoLog> GetPhytoLog(int entityId)
        {
            List<OrderRequestPhytoLog> orderRequestPhytoLogs = new List<OrderRequestPhytoLog>();
            //TODO
            return orderRequestPhytoLogs;
        }

        public int Insert(WebOrderRequest entity)
        {
            throw new NotImplementedException();
        }

        public int Insert(OrderRequest entity)
        {
            throw new NotImplementedException();
        }

        public List<OrderRequest> Search(OrderRequestSearch searchEntity)
        {
            List<OrderRequest> results = new List<OrderRequest>();

            SQL = " SELECT * FROM vw_GRINGlobal_Order_Request ";
            SQL += " WHERE      (@ID                        IS NULL OR  ID = @ID) ";
            SQL += " AND        (@CreatedByCooperatorID     IS NULL OR  CreatedByCooperatorID   = @CreatedByCooperatorID)";
            SQL += " AND        (@WebOrderRequestID         IS NULL OR  WebOrderRequestID       = @WebOrderRequestID)";

            var parameters = new List<IDbDataParameter> {
                CreateParameter("ID", searchEntity.ID > 0 ? (object)searchEntity.ID : DBNull.Value, true),
                CreateParameter("CreatedByCooperatorID", searchEntity.CreatedByCooperatorID > 0 ? (object)searchEntity.CreatedByCooperatorID : DBNull.Value, true),
                CreateParameter("WebOrderRequestID", searchEntity.WebOrderRequestID > 0 ? (object)searchEntity.WebOrderRequestID : DBNull.Value, true),
            };

            results = GetRecords<OrderRequest>(SQL, parameters.ToArray());
            RowsAffected = results.Count;

            return results;
        }

        public int Update(WebOrderRequest entity)
        {
            throw new NotImplementedException();
        }

        public int Update(OrderRequest entity)
        {
            throw new NotImplementedException();
        }
    }
}
